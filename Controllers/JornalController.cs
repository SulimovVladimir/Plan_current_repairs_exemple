using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Plan_current_repairs.Data.Interface;
using Plan_current_repairs.Data.Models;
using Plan_current_repairs.Data.VIewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Plan_current_repairs.Controllers
{
    [Authorize]
    public class JornalController : Controller
    {
        public readonly IJornal jornal;
        public readonly IWorks works;
        public readonly IYears years;
        public readonly IDepartment department;
        public readonly IDictionarySector dictionarySector;
        public readonly IAct acts;
        public readonly Settings settings;
        public readonly IStatus status;



        public JornalController(IJornal jornal, IWorks works, IYears years, IDepartment department, IDictionarySector dictionarySector, IStatus status, IAct acts, IOptionsMonitor<Settings> settings)
        {
            this.jornal = jornal;
            this.works = works;
            this.years = years;
            this.department = department;
            this.dictionarySector = dictionarySector;
            this.settings = settings.CurrentValue;
            this.status = status;
            this.acts = acts;
        }


        [HttpGet]
        public IActionResult IndexJornal(string Section, string Block, string Group, string Year, string Department)
        {

            if (Section == "Отчет" && Group != "Дополнительные работы, не предусмотренные планом") return RedirectToAction("SetJornalFact", new { Section = Section, Block = Block, Group = Group, Year = Year, Department = Department });
            if (Section == "План" && Group != "Дополнительные работы, не предусмотренные планом") return RedirectToAction("SetJornalPlan", new { Section = Section, Block = Block, Group = Group, Year = Year, Department = Department });
            if (Section == "Отчет" && Group == "Дополнительные работы, не предусмотренные планом") return RedirectToAction("SetOtherJornalFact", new { Section = Section, Block = Block, Group = Group, Year = Year, Department = Department });
            if (Section == "План" && Group == "Дополнительные работы, не предусмотренные планом")
            {
                TempData["Info"] = "\"Дополнительные работы, не предусмотренные планом\" заполняются только в отчете";
                return RedirectToAction("IndexJornal");
            }
            return View();
        }

        //заполнение отчетов
        [HttpGet]
        public IActionResult SetJornalFact(string Section, string Block, string Group, string Year, string Department)
        {
            HttpContext.Session.SetString("URL", $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Startup.NameProject}{HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            bool[] tempBlock = status.OnlyStatusRecord(Department, Year).Blocking_Array;
            string[] MonthOfBlock = null;
            bool flagBlock = false;
            string ConditionForSelectionAct = department.GetOnlyDepartmentID(department.GetOnlyDepartmentByName(Department)).StandartNumberAct + "-";
            switch (Block)
            {
                case "1-й квартал": { ConditionForSelectionAct += "I-"; MonthOfBlock = new string[] { "1-й квартал", "Январь", "Февраль", "Март" }; if (tempBlock[1]) flagBlock = true; break; }
                case "2-й квартал": { ConditionForSelectionAct += "II-"; MonthOfBlock = new string[] { "2-й квартал", "Апрель", "Май", "Июнь" }; if (tempBlock[2]) flagBlock = true; break; }
                case "3-й квартал": { ConditionForSelectionAct += "III-"; MonthOfBlock = new string[] { "3-й квартал", "Июль", "Август", "Сентябрь" }; if (tempBlock[3]) flagBlock = true; break; }
                case "4-й квартал": { ConditionForSelectionAct += "IV-"; MonthOfBlock = new string[] { "4-й квартал", "Октябрь", "Ноябрь", "Декабрь" }; if (tempBlock[4]) flagBlock = true; break; }
            }
            if (flagBlock && !(HttpContext.User.HasClaim("Role", "Admin") || HttpContext.User.HasClaim("ReviewerRole", "EngineerOfPTO")))
            {
                TempData["Info"] = $"Вкладка \"{Block}\" закрыта для редактирования. Обратитесь к администратору.";
                return RedirectToAction("IndexJornal");
            }
            ViewBag.Month = MonthOfBlock;
            ViewBag.Menu = new List<string> { Section, Block, Group, Year, Department };
            ViewBag.DictionarySector = dictionarySector.getDictionarySectorsOnlyDepartment(department.GetOnlyDepartmentByName(Department)).Select(x => x.Value);
            ViewBag.AllActForDepartment = acts.ListActForOnlyDepartment(department.GetOnlyDepartmentByName(Department), years.GetIDYearByName(Year), ConditionForSelectionAct).OrderByDescending(x => x.IDAct);
            IEnumerable<RecordsByJornal> jornalByFact = jornal.GetJornalByGroup(works.GetNameOfWorksOnlyGroup(Group).Where(x => x.Active == true), department.GetOnlyDepartmentByName(Department), years.GetIDYearByName(Year), ConditionForSelectionAct, true, false);
            return View(jornalByFact);
        }

        [HttpPost]
        public void SetJornalFact(List<Plan_current_repairs.Data.VIewModels.RecordsByJornal> jornalByFact, bool AddRecord)
        {
            foreach (var el in jornalByFact)
            {
                if (el.jornal != null)
                {
                    if (el.nameOfWorks.IntegerValue)
                    {
                        for (int i = 0; i < el.FactArray.Length; i++)
                        {
                            el.FactArray[i] = (float)(System.Math.Round(el.FactArray[i]));
                        }
                    }
                    try
                    {
                        jornal.EditFactValue(jornal.allRecordsJornal.FirstOrDefault(x => x.NumberRecordingID == el.jornal.NumberRecordingID), el.FactArray, el.FactParameters_1, el.FactParameters_2, el.FactParameters_3, el.jornal.Note, el.jornal.Sector);
                    }
                    catch (Exception)
                    {
                        TempData["Info"] = "Ошибка редактирования данных. Убедитесь, что использована \",\" для дробных чисел.";
                    }
                }
            }
            TempData["InfoOutTime"] = "Данные сохранены";
            if (AddRecord) return;
            Response.Redirect(HttpContext.Session.GetString("URL"));
        }

        //добавить запись в отчет
        public void AddRecordToJornal(string Department, string Year, int WorkID, List<Plan_current_repairs.Data.VIewModels.RecordsByJornal> jornalByFact, bool IsPlan)
        {
            if (IsPlan) SetJornalPlan(jornalByFact, true);
            else SetJornalFact(jornalByFact, true);
            jornal.AddRecordsToJornal(department.GetOnlyDepartmentByName(Department), years.GetIDYearByName(Year), WorkID, IsPlan);
            Response.Redirect(HttpContext.Session.GetString("URL"));
        }

        //создание акта выполненных работ с привязкой к записи
        public RedirectToActionResult CreateCardAct(int departmentID, int recordID, int yearID, string currentBlock, List<Plan_current_repairs.Data.VIewModels.RecordsByJornal> jornalByFact)
        {
            SetJornalFact(jornalByFact, true); //сохранение данных
            return RedirectToAction("CreateCardAct", "Act", new { departmentID = departmentID, recordID = recordID, yearID = yearID, currentBlock = currentBlock });
        }

        //добавление созданного акта к записи
        public void AddActToJornalRecord(int recordID, int actID, List<Plan_current_repairs.Data.VIewModels.RecordsByJornal> jornalByFact)
        {
            SetJornalFact(jornalByFact, true); //сохранение данных
            try
            {
                jornal.AddActToJornalRecord(recordID, 0, acts.GetOnlyActByID(actID));
            }
            catch { TempData["Info"] = "Не удалось прикрепить акт выполненных работ"; }
            Response.Redirect(HttpContext.Session.GetString("URL"));

        }

        //удаление записи из отчета
        public void DeleteRecordsFromJornal(int IDRecord, List<Plan_current_repairs.Data.VIewModels.RecordsByJornal> jornalByFact, bool IsPlan)
        {
            if (IsPlan) SetJornalPlan(jornalByFact, true);  //сохранение данных
            else SetJornalFact(jornalByFact, true);         //сохранение данных
            if (jornal.GetOnlyRecordByID(IDRecord).CreatedInPlan && !IsPlan) { TempData["Info"] = "Запрещено удалять запись, созданную в плане, из вкладки отчет."; }
            else { jornal.DeleteRecordsFromJornal(IDRecord); }
            Response.Redirect(HttpContext.Session.GetString("URL"));
        }

        //заполнение плана
        public IActionResult SetJornalPlan(string Section, string Block, string Group, string Year, string Department)
        {
            HttpContext.Session.SetString("URL", $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Startup.NameProject}{HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            bool[] tempBlock = status.OnlyStatusRecord(Department, Year).Blocking_Array;
            if (tempBlock[0] && !(HttpContext.User.HasClaim("Role", "Admin") || HttpContext.User.HasClaim("ReviewerRole", "EngineerOfPTO")))
            {
                TempData["Info"] = "Вкладка \"План\" закрыта для редактирования. Обратитесь к администратору.";
                return RedirectToAction("IndexJornal");
            }
            bool flagBlock = false;
            string[] MonthOfBlock = null;
            switch (Block)
            {
                case "1-й квартал": { MonthOfBlock = new string[] { "1-й квартал", "Январь", "Февраль", "Март" }; if (tempBlock[1]) flagBlock = true; break; }
                case "2-й квартал": { MonthOfBlock = new string[] { "2-й квартал", "Апрель", "Май", "Июнь" }; if (tempBlock[2]) flagBlock = true; break; }
                case "3-й квартал": { MonthOfBlock = new string[] { "3-й квартал", "Июль", "Август", "Сентябрь" }; if (tempBlock[3]) flagBlock = true; break; }
                case "4-й квартал": { MonthOfBlock = new string[] { "4-й квартал", "Октябрь", "Ноябрь", "Декабрь" }; if (tempBlock[4]) flagBlock = true; break; }
            }

            if (flagBlock && !(HttpContext.User.HasClaim("Role", "Admin") || HttpContext.User.HasClaim("ReviewerRole", "EngineerOfPTO")))
            {
                TempData["Info"] = $"Вкладка \"{Block}\" закрыта для редактирования. Обратитесь к администратору.";
                return RedirectToAction("IndexJornal");
            }
            ViewBag.Month = MonthOfBlock;
            ViewBag.Menu = new List<string> { Section, Block, Group, Year, Department };
            ViewBag.DictionarySector = dictionarySector.getDictionarySectorsOnlyDepartment(department.GetOnlyDepartmentByName(Department)).Select(x => x.Value);
            IEnumerable<RecordsByJornal> jornalByPlan = jornal.GetJornalByGroup(works.GetNameOfWorksOnlyGroup(Group).Where(x => x.Active == true), department.GetOnlyDepartmentByName(Department), years.GetIDYearByName(Year), "", false, false);
            return View(jornalByPlan);
        }

        [HttpPost]
        public void SetJornalPlan(List<Plan_current_repairs.Data.VIewModels.RecordsByJornal> jornalByPlan, bool AddRecord)
        {
            foreach (var el in jornalByPlan)
            {
                if (el.jornal != null)
                {
                    if (el.nameOfWorks.IntegerValue)
                    {
                        for (int i = 0; i < el.PlanArray.Length; i++)
                        {
                            el.PlanArray[i] = (float)(System.Math.Round(el.PlanArray[i]));
                        }
                    }
                    try
                    {
                        jornal.EditPlanValue(jornal.allRecordsJornal.FirstOrDefault(x => x.NumberRecordingID == el.jornal.NumberRecordingID), el.PlanArray, el.PlanParameters_1, el.PlanParameters_2, el.PlanParameters_3, el.jornal.Note, el.jornal.Sector);
                    }
                    catch (Exception)
                    {
                        TempData["Info"] = "Ошибка редактирования данных. Убедитесь, что использована \",\" для дробных чисел.";
                    }
                }
            }
            TempData["InfoOutTime"] = "Данные сохранены";
            if (AddRecord) return;
            Response.Redirect(HttpContext.Session.GetString("URL"));
        }

        //заполнение прочих работ
        [HttpGet]
        public IActionResult SetOtherJornalFact(string Section, string Block, string Group, string Year, string Department)
        {
            HttpContext.Session.SetString("URL", $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Startup.NameProject}{HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            bool[] tempBlock = status.OnlyStatusRecord(Department, Year).Blocking_Array;
            bool flagBlock = false;
            string[] MonthOfBlock = null;
            string ConditionForSelectionAct = department.GetOnlyDepartmentID(department.GetOnlyDepartmentByName(Department)).StandartNumberAct + "-";
            switch (Block)
            {
                case "1-й квартал": { ConditionForSelectionAct += "I-"; MonthOfBlock = new string[] { "1-й квартал", "Январь", "Февраль", "Март" }; if (tempBlock[1]) flagBlock = true; break; }
                case "2-й квартал": { ConditionForSelectionAct += "II-"; MonthOfBlock = new string[] { "2-й квартал", "Апрель", "Май", "Июнь" }; if (tempBlock[2]) flagBlock = true; break; }
                case "3-й квартал": { ConditionForSelectionAct += "III-"; MonthOfBlock = new string[] { "3-й квартал", "Июль", "Август", "Сентябрь" }; if (tempBlock[3]) flagBlock = true; break; }
                case "4-й квартал": { ConditionForSelectionAct += "IV-"; MonthOfBlock = new string[] { "4-й квартал", "Октябрь", "Ноябрь", "Декабрь" }; if (tempBlock[4]) flagBlock = true; break; }
            }
            if (flagBlock && !(HttpContext.User.HasClaim("Role", "Admin") || HttpContext.User.HasClaim("ReviewerRole", "EngineerOfPTO")))
            {
                TempData["Info"] = $"Вкладка \"{Block}\" закрыта для редактирования. Обратитесь к администратору.";
                return RedirectToAction("IndexJornal");
            }
            ViewBag.Month = MonthOfBlock;
            ViewBag.Menu = new List<string> { Section, Block, Group, Year, Department };
            ViewBag.AllActForDepartment = acts.ListActForOnlyDepartment(department.GetOnlyDepartmentByName(Department), years.GetIDYearByName(Year), ConditionForSelectionAct).OrderByDescending(x => x.IDAct);
            IEnumerable<OtherJornalByFact> otherJornalByFact = jornal.GetOtherJornalByFacts(department.GetOnlyDepartmentByName(Department), years.GetIDYearByName(Year), ConditionForSelectionAct);
            return View(otherJornalByFact);
        }


        [HttpPost]
        public void SetOtherJornalFact(List<Plan_current_repairs.Data.VIewModels.OtherJornalByFact> otherJornalByFact, bool AddRecord)
        {
            foreach (var el in otherJornalByFact)
            {
                try
                {
                    jornal.EditOtherFactValue(jornal.allRecordsJornalOtherWork.FirstOrDefault(x => x.OtherWorkID == el.otherWork.OtherWorkID), el.OtherFactArray, el.otherWork.NoteOtherWork);
                }
                catch (Exception)
                {
                    TempData["Info"] = "Ошибка редактирования данных. Убедитесь, что использована \",\" для дробных чисел.";
                }
            }
            if (AddRecord) return;
            Response.Redirect(HttpContext.Session.GetString("URL"));
        }

        //редирект на добавление прочей работы
        public RedirectToActionResult TempAddOtherWork(string department, string year, List<Plan_current_repairs.Data.VIewModels.OtherJornalByFact> otherJornalByFact)
        {
            SetOtherJornalFact(otherJornalByFact, true);    //сохранение данных
            return RedirectToAction("AddOtherWork", new { Department = department, Year = year });
        }

        //добавление прочей работы
        public IActionResult AddOtherWork(string Department, string Year)
        {
            ViewBag.DepartmentID = department.GetOnlyDepartmentByName(Department);
            ViewBag.YearID = years.GetIDYearByName(Year);
            return View();
        }

        [HttpPost]
        public void AddOtherWork(OtherWork otherWork)
        {
            jornal.AddRecordsOtherWork(otherWork);
            Response.Redirect(HttpContext.Session.GetString("URL"));
        }

        //Удаление прочей работы
        public void DeleteRecordOtherWork(int OtherWorkID)
        {
            if (OtherWorkID != 0)
            {
                jornal.DeleteRecordOtherWork(OtherWorkID);
                Response.Redirect(HttpContext.Session.GetString("URL"));
            }
        }

        //создание Акта ВР с привязкой к записи прочей работы
        public RedirectToActionResult CreateCardActByOtherWork(int departmentID, int recordOtherID, int yearID, string currentBlock, List<Plan_current_repairs.Data.VIewModels.OtherJornalByFact> otherJornalByFact)
        {
            SetOtherJornalFact(otherJornalByFact, true);
            return RedirectToAction("CreateCardAct", "Act", new { departmentID = departmentID, recordOtherID = recordOtherID, yearID = yearID, currentBlock = currentBlock });
        }

        //добавление Акта ВР к записи прочей работы
        public void AddActToJornalOtherWork(int recordOhterID, int actID, List<Plan_current_repairs.Data.VIewModels.OtherJornalByFact> otherJornalByFact)
        {
            SetOtherJornalFact(otherJornalByFact, true);
            try
            {
                jornal.AddActToJornalRecord(0, recordOhterID, acts.GetOnlyActByID(actID));
            }
            catch { TempData["Info"] = "Не удалось прикрепить акт выполненных работ"; }
            Response.Redirect(HttpContext.Session.GetString("URL"));

        }

    }
}
