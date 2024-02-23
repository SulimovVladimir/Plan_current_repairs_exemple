using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Plan_current_repairs.Data.Interface;
using Plan_current_repairs.Data.Models;
using Plan_current_repairs.Data.VIewModels;
using System.Linq;



namespace Plan_current_repairs.Controllers.Admin
{
    //управление словарями подразделений
    [Authorize]
    public class AdminDictionarySectorController : Controller
    {
        public readonly IDepartment _department;
        public readonly IEmployee _employee;
        public readonly IDictionarySector _dictionarySector;
        public AdminDictionarySectorController(IDepartment department, IDictionarySector dictionarySector, IEmployee employee)
        {
            _department = department;
            _employee = employee;
            _dictionarySector = dictionarySector;
        }
        public ActionResult IndexDictionarySector(int currentDepartment = 0)
        {
            DictionaryModel dictionaryModel=new DictionaryModel();
            if (currentDepartment != 0)
            {
              dictionaryModel = new DictionaryModel()
                {
                    dictionarySector = new DictionarySector() {DepartmentID=currentDepartment},
                    listDictionarySectorsByOnlyDepartment = _dictionarySector.getDictionarySectorsOnlyDepartment(currentDepartment),
                    currentDepartment = currentDepartment,
                };
            }
            if(HttpContext.User.HasClaim("Role", "Admin")) dictionaryModel.allDepartment = new SelectList(_department.allDepartment.Where(x => x.ID != 1), "ID", "NameDepartment");
            return View(dictionaryModel);
        }
  
        public ActionResult AddDictionarySector(DictionaryModel dictionaryModel)
        {
            if(HttpContext.User.HasClaim("Role", "Admin") || HttpContext.User.FindFirst("IDDepartment").Value==dictionaryModel.dictionarySector.DepartmentID.ToString())
            {
                _dictionarySector.AddAndEditDictionarySector(dictionaryModel.dictionarySector);
                return RedirectToAction("IndexDictionarySector", new { currentDepartment = dictionaryModel.dictionarySector.DepartmentID });
            }
            else
            {
                TempData["Info"] = "Нет прав на добавление записей в словарь участков";
                return RedirectToAction("IndexDictionarySector", new {currentDepartment=HttpContext.User.FindFirst("IDDepartment").Value});
            }
               
        }


        public ActionResult AddDictionarySectorByJornal(string departmentDS, string valueDS )
        {

            _dictionarySector.AddAndEditDictionarySector(new DictionarySector {DepartmentID=_department.GetOnlyDepartmentByName(departmentDS), Value=valueDS});
            return Redirect(HttpContext.Session.GetString("URL"));
        }

        public ActionResult DeleteSector(int DS, int currentDepartment)
        {
            if (HttpContext.User.HasClaim("Role", "Admin") || HttpContext.User.FindFirst("IDDepartment").Value == currentDepartment.ToString())
            {
                TempData["Info"] = $"{_dictionarySector.GetOnlyDictionarySectorID(DS).Value} удалено";
                _dictionarySector.DeleteDictionarySector(DS);
                return RedirectToAction("IndexDictionarySector", new { currentDepartment = currentDepartment });
            }
            else
            {
                TempData["Info"] = "Нет прав на удаление записей из словаря участков";
                return RedirectToAction("IndexDictionarySector", new { currentDepartment = HttpContext.User.FindFirst("IDDepartment").Value });
            }
        }
    }
}
