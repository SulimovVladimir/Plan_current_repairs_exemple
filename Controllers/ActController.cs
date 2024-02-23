using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Plan_current_repairs.Data.Interface;
using Plan_current_repairs.Data.Models;
using Plan_current_repairs.Data.VIewModels;
using Quartz.Logging;
using Quartz.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web.Razor.Parser.SyntaxTree;

namespace Plan_current_repairs.Controllers
{
    public class ActController : Controller
    {
        public readonly IAct acts;
        public readonly Settings settings;
        public readonly IJornal jornals;
        public readonly IDepartment departments;
        public readonly IYears years;
        public readonly IEmployee employees;
        public readonly IStatus statuses;
        public readonly ILogger<ActController> logger;
        public ActController(IAct _acts, IJornal _jornals, IDepartment _departments, IYears _years, IEmployee _employees,IStatus _statuses, IOptionsMonitor<Settings> _settings, ILogger<ActController> _logger)
        {
            acts = _acts;
            jornals = _jornals;
            departments = _departments;
            years = _years;
            employees = _employees;
            statuses = _statuses;
            settings = _settings.CurrentValue;
            logger = _logger;
        }

        public ActionResult IndexAct(string numberAct = null, string department = null, string year = null)
        {
            HttpContext.Session.SetString("URL", $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Startup.NameProject}{HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            string currentDepartment = null;
            if (department.IsNullOrWhiteSpace())
            {
                if (HttpContext.User.Identity.IsAuthenticated && employees.GetAllEmployee.Any(x => x.Account == HttpContext.User.Identity.Name && x.DepartmentID != 1))
                {
                    currentDepartment = employees.GetAllEmployee.FirstOrDefault(x => x.Account == HttpContext.User.Identity.Name).Department.NameDepartment;
                }
            }
            else currentDepartment = department;

            string currentYear = year.IsNullOrWhiteSpace() ? settings.CurrentYear.ToString() : year;

            ArchiveAct archiveAct = new ArchiveAct()
            {
                ListAct = currentDepartment.IsNullOrWhiteSpace() ? null : acts.ListActForOnlyDepartment(departments.GetOnlyDepartmentByName(currentDepartment), years.GetIDYearByName(currentYear), "I").OrderBy(x => x.NumberAct.Length).ThenBy(x => x.NumberAct),
                Departments = new SelectList(departments.allDepartmentWithoutAdministration, "ID", "NameDepartment"),
                Years = new SelectList(years.GetAllYears, "YearID", "Years"),
                CurrentYear = currentYear,
                CurrentDepartment = currentDepartment,
                CurrentAct = numberAct
            };
            if (HttpContext.User.HasClaim("Role", "Admin") | HttpContext.User.HasClaim("ReviewerRole", "EngineerOfPTO"))
            {
                ViewBag.NotBlockingBlock = new bool[] { false, false, false, false, false };
                return View(archiveAct);
            }
            if(HttpContext.User.Identity.IsAuthenticated && !(HttpContext.User.HasClaim("ReviewerRole", "HeadOfPTO") || HttpContext.User.HasClaim("ReviewerRole", "ChiefEngineer"))) ViewBag.NotBlockingBlock = statuses.OnlyStatusRecord(archiveAct.CurrentDepartment, archiveAct.CurrentYear).Blocking_Array;
            else ViewBag.NotBlockingBlock = new bool[] {true, true, true, true, true};
            return View(archiveAct);
        }

        //создать Акт ВР с привязкой к записи
        [Authorize]
        public ActionResult CreateCardAct(int departmentID, int recordID, int recordOtherID, int yearID, string currentBlock)
        {
            string Block = null;
            switch (currentBlock)
            {
                case "1-й квартал": { Block = "I"; break; }
                case "2-й квартал": { Block = "II"; break; }
                case "3-й квартал": { Block = "III"; break; }
                case "4-й квартал": { Block = "IV"; break; }
            }

            if (Block.IsNullOrWhiteSpace())
            {
                TempData["Info"] = "Не определен квартал составления акта";
                return RedirectToAction("IndexAct");
            }

            CardAct cardAct = null;
            if (recordID != 0) cardAct = new CardAct()
            {
                actOfWork = new ActOfWork { DepartmentID = departmentID, Department = departments.GetOnlyDepartmentID(departmentID), Jornals = new List<Jornal> { jornals.GetOnlyRecordByID(recordID) }, DateAct = DateTime.Today, Year = years.GetOnlyYearByID(yearID), YearID = yearID },
                RecordID = recordID,
                Block = Block
            };
            else cardAct = new CardAct()
            {
                actOfWork = new ActOfWork { DepartmentID = departmentID, Department = departments.GetOnlyDepartmentID(departmentID), OtherWorks = new List<OtherWork> { jornals.GetOnlyRecordOtherWorkByID(recordOtherID) }, DateAct = DateTime.Today, Year = years.GetOnlyYearByID(yearID), YearID = yearID },
                RecordOtherID = recordOtherID,
                Block = Block
            };

            ViewBag.AllEmployee = employees.GetAllEmployeeByDepartment(departmentID);
            return View("CardAct", cardAct);
        }

        //создание Акта ВР без привязки к журналу
        [Authorize]
        public ActionResult CreateCardActWithoutJornal(string department, string year, string currentBlock)
        {
           
            string Block = null;
            switch (currentBlock)
            {
                case "1-й квартал": { Block = "I"; break; }
                case "2-й квартал": { Block = "II"; break; }
                case "3-й квартал": { Block = "III"; break; }
                case "4-й квартал": { Block = "IV"; break; }
            }

            if (Block.IsNullOrWhiteSpace())
            {
                TempData["Info"] = "Не определен квартал составления акта";
                return RedirectToAction("IndexAct");
            }
            int departmentID;
            try { departmentID = departments.GetOnlyDepartmentByName(department); }
            catch {
                TempData["Info"] = "Выберите подразделение";
                return RedirectToAction("IndexAct");
            }
            
            int yearID = years.GetIDYearByName(year);
            CardAct cardAct = new CardAct()
            {
                actOfWork=new ActOfWork 
                { 
                    DepartmentID = departmentID, 
                    Department = departments.GetOnlyDepartmentID(departmentID),
                    DateAct = DateTime.Today, 
                    Year = years.GetOnlyYearByID(yearID), 
                    YearID = yearID },
                Block = Block 
                };
            
            ViewBag.AllEmployee = employees.GetAllEmployeeByDepartment(departmentID);
            return View("CardAct", cardAct);
        }
        public ActionResult CardAct() 
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult CardAct(CardAct cardAct)
        {
            if(cardAct.actOfWork.NumberAct==null)
            {
                TempData["Info"] = "Данный квартал уже заблокирован";
                return Redirect(HttpContext.Session.GetString("URL"));
            }

            int flagID = cardAct.actOfWork.IDAct;
            if (!(employees.GetOnlyEmployee(HttpContext.User.Identity.Name).DepartmentID == cardAct.actOfWork.DepartmentID || HttpContext.User.HasClaim("Role", "Admin") || HttpContext.User.HasClaim("ReviewerRole", "EngineerOfPTO")))
            {
                TempData["Info"] = "Вы не можете вносить акты выполненных работ другого филиала.";
                return Redirect(HttpContext.Session.GetString("URL"));
            }

            bool[] blockStatus = statuses.OnlyStatusRecord(cardAct.actOfWork.DepartmentID, cardAct.actOfWork.YearID).Blocking_Array;
            bool IsBlock = false;
            switch(cardAct.Block)
            {
                case "I": { if(blockStatus[1]) IsBlock=true ;break;}
                case "II": { if (blockStatus[2]) IsBlock = true; break;}
                case "III": { if (blockStatus[3]) IsBlock = true; break;}
                case "IX": { if (blockStatus[4]) IsBlock = true; break; }
            }

            if(!(!IsBlock || HttpContext.User.HasClaim("Role", "Admin") || HttpContext.User.HasClaim("ReviewerRole", "EngineerOfPTO")))
            {
                TempData["Info"] = "Данный квартал уже заблокирован.";
                return Redirect(HttpContext.Session.GetString("URL"));
            }

            if (cardAct.actOfWork.IDAct==0) cardAct.actOfWork.NumberAct=departments.GetOnlyDepartmentID(cardAct.actOfWork.DepartmentID).StandartNumberAct+"-"+cardAct.Block+"-" + cardAct.actOfWork.NumberAct;

            if (years.GetOnlyYearByID(cardAct.actOfWork.YearID).Years != cardAct.actOfWork.DateAct.Year && cardAct.actOfWork.IDAct == 0)
                TempData["Info"] = "Дата акта выполненных работ не соответствует текущему году";
            else if (!years.IsCorrectActByNumber(cardAct.actOfWork.NumberAct, cardAct.actOfWork.YearID) && cardAct.actOfWork.IDAct == 0)
                TempData["Info"] = "Данный номер акта выполненных работ уже существует";
            else
            {
                acts.AddAndEditAct(cardAct.actOfWork, cardAct.RecordID, cardAct.RecordOtherID);
                if (flagID == 0)
                    logger.LogInformation("Пользователь {user} добавил акт выполненных работ №{numberAсt} (ID:{IDAct})", HttpContext.User.Identity.Name, cardAct.actOfWork.NumberAct, cardAct.actOfWork.IDAct);
                else logger.LogInformation("Пользователь {user} изменил акт выполненных работ №{numberAсt} (ID:{IDAct})", HttpContext.User.Identity.Name, cardAct.actOfWork.NumberAct, cardAct.actOfWork.IDAct);
            }
            try
            {
                var uri = new Uri(HttpContext.Session.GetString("URL"));
                return Redirect(uri.AbsoluteUri);
            }
            catch (Exception)
            {
                return View("IndexAct");
            }
        }

        [Authorize]
        public ActionResult EditCardAct(int ActID, bool isArchive)
        {
            if(isArchive) 
            {
                string Url = HttpContext.Session.GetString("URL");
                if(Url.Contains("&numberAct"))
                {
                    Url = Url.Remove(Url.LastIndexOf('&'));
                }
                if (Url.Contains("?numberAct"))
                {
                    Url = Url.Remove(Url.LastIndexOf('?'));
                }
                if (Url.Contains("?")) HttpContext.Session.SetString("URL", $"{Url}&numberAct={acts.GetOnlyActByID(ActID).NumberAct}");
                else HttpContext.Session.SetString("URL", $"{Url}?numberAct={acts.GetOnlyActByID(ActID).NumberAct}");
            }

            ActOfWork tempAct = acts.GetOnlyActByID(ActID);

            if (tempAct.IsCloseEditAndRemove && !(HttpContext.User.HasClaim("Role", "Admin") || HttpContext.User.HasClaim("ReviewerRole", "EngineerOfPTO")))
            {
                TempData["Info"] = $"Редактирование данного акта выполненных работ заблокировано. Обратитесь к администратуру системы";
                return RedirectToAction("Index","Home");
            }

            if (!HttpContext.User.HasClaim("Department", tempAct.Department.NameDepartment) && !(HttpContext.User.HasClaim("Role", "Admin") || HttpContext.User.HasClaim("ReviewerRole", "EngineerOfPTO")))
            {
                TempData["Info"] = $"Вы не можете редактировать акты выполненных работ другого филиала ({tempAct.Department.NameDepartment}).";
                return RedirectToAction("Index", "Home");
            }
            ViewBag.AllEmployee = employees.GetAllEmployeeByDepartment(tempAct.DepartmentID);
            return View("CardAct", new CardAct{actOfWork = tempAct, RecordID=0 } );
        }

        public ActionResult ViewCardAct(int ActID)
        {
            ActOfWork tempAct = acts.GetOnlyActByID(ActID);
            return View(tempAct);
        }

        [Authorize]
        public RedirectResult DeleteCardAct(int ActID) 
        {
            ActOfWork tempAct = acts.GetOnlyActByID(ActID);

            if (tempAct.IsCloseEditAndRemove && !(HttpContext.User.HasClaim("Role", "Admin") || HttpContext.User.HasClaim("ReviewerRole", "EngineerOfPTO")))
            {
                TempData["Info"] = $"Удаление данного акта выполненных работ заблокировано. Обратитесь к администратуру системы";
                return Redirect(HttpContext.Session.GetString("URL"));
            }
            if (!HttpContext.User.HasClaim("Department", tempAct.Department.NameDepartment) && !(HttpContext.User.HasClaim("Role", "Admin") || HttpContext.User.HasClaim("ReviewerRole", "EngineerOfPTO")))
            {
                TempData["Info"] = $"Вы не можете удалять акты выполненных работ другого филиала ({tempAct.Department.NameDepartment}).";
                return Redirect("/Home/Index"); 
                
            }
            acts.DeleteAct(tempAct);
            logger.LogInformation("Пользователь {user} удалил акт выполненных работ №{numberAct} (ID:{IDAct})", HttpContext.User.Identity.Name, tempAct.NumberAct, tempAct.IDAct);
            return Redirect(HttpContext.Session.GetString("URL"));
        }

        //отвязать Акт ВР от записи
        [Authorize]
        public RedirectResult UnpinCardAct(int ActID, int RecordID, int OtherRecordID)
        {
            acts.UnpinCardActFromRecord(ActID, RecordID, OtherRecordID);
            return Redirect(HttpContext.Session.GetString("URL"));
        }
    }
}
 