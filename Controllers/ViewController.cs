using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Plan_current_repairs.Data.Interface;
using Plan_current_repairs.Data.VIewModels;
using System.Collections.Generic;
using System.Linq;
using Plan_current_repairs.Data.Models;
using Microsoft.Extensions.Options;

namespace Plan_current_repairs.Controllers
{
    public class ViewController : Controller
    {
        public readonly IJornal jornal;
        public readonly IWorks works;
        public readonly IYears years;
        public readonly IDepartment department;
        public readonly Settings settings;



        public ViewController(IJornal jornal, IWorks works, IYears years, IDepartment department, IOptionsMonitor<Settings> settings)
        {
            this.jornal = jornal;
            this.works = works;
            this.years = years;
            this.department = department;
            this.settings = settings.CurrentValue;
            
        }


        public IActionResult IndexViewDepartment(string Section, string Block, string Group, string Year, string Department, bool master, bool OpenTitle, bool CollapseBlock, bool ShowNullValue)
        {

           if (Section!= null) return RedirectToAction("ShowJornalDepartment", new { Section = Section, Block = Block, Group = Group, Year = Year, Department = Department, master=master, OpenTitle=OpenTitle, CollapseBlock=CollapseBlock, ShowNullValue= ShowNullValue});
           
            ViewBag.master = master;
            return View();
        }


        public IActionResult ShowJornalDepartment (string Section, string Block, string Group, string Year, string Department, bool master, bool OpenTitle, bool CollapseBlock, bool ShowNullValue)
        {

            HttpContext.Session.SetString("URL", $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Startup.NameProject}{HttpContext.Request.Path}{HttpContext.Request.QueryString}");

            ViewBag.Menu = new List<string> { Section, Block, Group, Year, Department };
            ViewBag.Flags = new List<bool> { OpenTitle, CollapseBlock, ShowNullValue };
            ViewBag.master = master;
            ViewBag.settings = new List<string> { settings.ChiefEngineer_Post, settings.ChiefEngineer_Name, settings.HeadOfPTO_Post, settings.HeadOfPTO_Name };
            IEnumerable<RecordsByJornal> jornalByPlanView = null; 

                string ConditionForSelectionAct = department.GetOnlyDepartmentID(department.GetOnlyDepartmentByName(Department)).StandartNumberAct + "-";
                switch (Block)
                {
                    case "1-й квартал": { ConditionForSelectionAct += "I-"; break; }
                    case "2-й квартал": { ConditionForSelectionAct += "II-"; break; }
                    case "3-й квартал": { ConditionForSelectionAct += "III-"; break; }
                    case "4-й квартал": { ConditionForSelectionAct += "IV-"; break; }
                }
            
            if (Group == "Все группы") 
            { 
                jornalByPlanView = jornal.GetJornalByGroup(works.AllNameOfWorks.Where(x => x.Active == true), department.GetOnlyDepartmentByName(Department), years.GetIDYearByName(Year), ConditionForSelectionAct, true, false); 
            }
            else 
            {
                jornalByPlanView =jornal.GetJornalByGroup(works.GetNameOfWorksOnlyGroup(Group).Where(x => x.Active == true), department.GetOnlyDepartmentByName(Department), years.GetIDYearByName(Year), ConditionForSelectionAct, true, false); 
            }
            if(Group == "Дополнительные работы, не предусмотренные планом" || Group == "Все группы")
            ViewBag.OtherWork = jornal.GetOtherJornalByFacts(department.GetOnlyDepartmentByName(Department), years.GetIDYearByName(Year),ConditionForSelectionAct);
            var GroupJornalByPlanView = jornalByPlanView.GroupBy(x => x.nameOfWorks.GroupName);
            return View(GroupJornalByPlanView);
        }

        public IActionResult IndexViewMaster(string Section, string Block, string Group, string Year, string Department, bool OpenTitle, bool CollapseBlock, bool ShowNullValue, bool master=true)
        {

            if (Section != null) return RedirectToAction("ShowJornalMaster", new { Section = Section, Block = Block, Group = Group, Year = Year, Department = Department, master = master, OpenTitle = OpenTitle, CollapseBlock = CollapseBlock, ShowNullValue= ShowNullValue });

            ViewBag.master = master;
            return View();
        }

        public IActionResult ShowJornalMaster(string Section, string Block, string Group, string Year, bool master, bool OpenTitle, bool CollapseBlock,bool ShowNullValue, string Department="УС")
        {

            HttpContext.Session.SetString("URL", $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}{HttpContext.Request.QueryString}");

            ViewBag.Menu = new List<string> { Section, Block, Group, Year, Department };
            ViewBag.Flags = new List<bool> { OpenTitle, CollapseBlock, ShowNullValue };
            ViewBag.master = master;
            ViewBag.settings = new List<string> { settings.ChiefEngineer_Post, settings.ChiefEngineer_Name, settings.HeadOfPTO_Post, settings.HeadOfPTO_Name };
            IEnumerable<RecordsByJornal> jornalByPlanView = null;

            if (Group == "Все группы")
            {
                jornalByPlanView = jornal.GetJornalByGroup(works.AllNameOfWorks.Where(x => x.Active == true),0, years.GetIDYearByName(Year),"", true, true);
            }
            else
            {
                jornalByPlanView = jornal.GetJornalByGroup(works.GetNameOfWorksOnlyGroup(Group).Where(x => x.Active == true), 0, years.GetIDYearByName(Year),"", true, true);
            }
           
            var GroupJornalByPlanView = jornalByPlanView.GroupBy(x => x.nameOfWorks.GroupName);
            return View(GroupJornalByPlanView);
        }

    }
}
