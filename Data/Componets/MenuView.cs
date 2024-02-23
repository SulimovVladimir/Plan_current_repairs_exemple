using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Plan_current_repairs.Data.Interface;
using Plan_current_repairs.Data.VIewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Plan_current_repairs.Data.Componets
{
    public class MenuView : ViewComponent
    {
        public readonly IDepartment department;
        public readonly IJornal jornal;
        public readonly IWorks works;
        public readonly IYears years;


        public MenuView(IJornal jornal, IWorks works, IYears years, IDepartment department)
        {

            this.jornal = jornal;
            this.works = works;
            this.years = years;
            this.department = department;
        }

        public IViewComponentResult Invoke(string Section = null, string Block = "Весь год", string Group = null, string Year = null, string Department = null, bool master=false)
        {

            MenuModel menuViewModel = new MenuModel();

            menuViewModel.Departments = new SelectList(department.allDepartmentWithoutAdministration, "ID", "NameDepartment");
            menuViewModel.Years = new SelectList(years.GetAllYears, "YearID", "Years");
            var itemsSection = master ? new List<string>{"Сводный отчет","Сводный план"}: new List<string>{"Отчет","План"};
            menuViewModel.Sections = new SelectList(itemsSection);
            var itemsBlock = new List<string> { "Весь год", "1-й квартал", "2-й квартал", "3-й квартал", "4-й квартал" };
            menuViewModel.Blocks = new SelectList(itemsBlock);
            menuViewModel.Groups = master ? new SelectList(works.AllGroupNameOfWorks().Where(x=>x.NameGroup!= "Дополнительные работы, не предусмотренные планом"), "GroupID", "NameGroup"): new SelectList(works.AllGroupNameOfWorks(), "GroupID", "NameGroup");
            
            try
            {
                if(Department != "УС")
                menuViewModel.CurrentDepartment = (Department != null) ? menuViewModel.Departments.FirstOrDefault(p => p.Text == Department).Text : menuViewModel.Departments.First().Text;
                menuViewModel.CurrentSection = (Section != null) ? menuViewModel.Sections.FirstOrDefault(p => p.Text == Section).Text : menuViewModel.Sections.First().Text;
                menuViewModel.CurrentBlock = Block;
                menuViewModel.CurrentGroup = (Group != null) ? Group : menuViewModel.Groups.First().Text;
                menuViewModel.CurrentYear = (Year != null) ? menuViewModel.Years.FirstOrDefault(p => p.Text == Year).Text : menuViewModel.Years.Last().Text;
            }
            catch (Exception)
            {
                TempData["Info"] = "Установлены не все вводные данные. Обратитесь к администратору";
                return Content("");
            }

            menuViewModel.Master = master;
            return View("_MenuView", menuViewModel);
        }
    }
}

