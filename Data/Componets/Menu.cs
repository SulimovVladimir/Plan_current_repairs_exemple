using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Plan_current_repairs.Data.Interface;
using Plan_current_repairs.Data.Models;
using Plan_current_repairs.Data.VIewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan_current_repairs.Data.Componets
{
 
    public class Menu : ViewComponent
    {
        public readonly IDepartment department;
        public readonly IJornal jornal;
        public readonly IWorks works;
        public readonly IYears years;
        public readonly IEmployee employees;
        public readonly Settings settings;


        public Menu(IJornal jornal, IWorks works, IYears years, IDepartment department, IEmployee employees, IOptionsMonitor<Settings> _settings)
        {

            this.jornal = jornal;
            this.works = works;
            this.years = years;
            this.department = department;
            this.employees = employees;
            settings = _settings.CurrentValue;
        }

        public IViewComponentResult Invoke(string Section = "Отчет", string Block = "1-й квартал", string Group = null, string Year = null, string Department = null)
        {
            if (employees.GetOnlyEmployee(HttpContext.User.Identity.Name).Role == "Рецензент" &&
                employees.GetOnlyEmployee(HttpContext.User.Identity.Name).DepartmentID==1 &&
                employees.GetOnlyEmployee(HttpContext.User.Identity.Name).FullName!=settings.EngineerOfPTO_Name)
            {
                TempData["Info"] = "Рецензентам доступна только вкладка \"Просмотр\"";
                return Content("");
            }

            MenuModel menuModel = new MenuModel();

            menuModel.Departments = employees.AdminIsTrue(HttpContext.User.Identity.Name)? 
                                    new SelectList(department.allDepartmentWithoutAdministration, "ID", "NameDepartment"):   
                                    new SelectList(department.GetOnlyDepartmentByUser(employees.GetOnlyEmployee(HttpContext.User.Identity.Name)), "ID", "NameDepartment");
            menuModel.Years = new SelectList(years.GetAllYears, "YearID", "Years");
            var itemsSection = new List<string> { "План", "Отчет" };
            menuModel.Sections = new SelectList(itemsSection);
            var itemsBlock = new List<string> { "1-й квартал", "2-й квартал", "3-й квартал", "4-й квартал" };
            menuModel.Blocks = new SelectList(itemsBlock);
            menuModel.Groups = new SelectList(works.AllGroupNameOfWorks(), "GroupID", "NameGroup");
            try
            {
                menuModel.CurrentDepartment = (Department != null) ? menuModel.Departments.FirstOrDefault(p => p.Text == Department).Text : menuModel.Departments.First().Text;
                menuModel.CurrentSection = menuModel.Sections.FirstOrDefault(p => p.Text == Section).Text;
                menuModel.CurrentBlock = Block;
                menuModel.CurrentGroup = (Group != null) ? menuModel.Groups.FirstOrDefault(p => p.Text == Group).Text : menuModel.Groups.First().Text;
                menuModel.CurrentYear = (Year != null) ? menuModel.Years.FirstOrDefault(p => p.Text == Year).Text : menuModel.Years.Last().Text;
            }
            catch (Exception)
            {
                TempData["Info"] = "Установлены не все вводные данные. Обратитесь к администратору";
                return Content("");
            }
          

            return View("_MenuPlan",menuModel);
        }
    }
}
