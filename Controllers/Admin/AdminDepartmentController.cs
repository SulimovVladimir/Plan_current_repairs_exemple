using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Plan_current_repairs.Data.Interface;
using Plan_current_repairs.Data.Models;


namespace Plan_current_repairs.Controllers
{
   
    [Authorize("IsAdmin")]
    public class AdminDepartmentController :Controller
    {
        public readonly IDepartment department;
        public readonly IEmployee employee;
        public AdminDepartmentController(IDepartment department, IEmployee employee)
        {
            this.department = department;
            this.employee = employee;
        }
        public IActionResult IndexDepartment()
        {
            ViewBag.AllDepartment = department.allDepartment;
            return View();
        }

        public IActionResult CardDepartment()
        {
            ViewBag.Action = "Добавить";
            return View();
        }

        [HttpPost]
        public IActionResult CardDepartment(Department department)
        {
            this.department.AddAndEditDepartment(department);
            return RedirectToAction("IndexDepartment", "AdminDepartment");
        } 

        public IActionResult DeleteDepartment(int DepartmentID)
        {
            if (DepartmentID != 0)
            {
                TempData["Info"] = $"{department.GetOnlyDepartmentID(DepartmentID).NameDepartment} удалено";
                department.DeleteDepartment(DepartmentID);
            }
            else TempData["Info"] = "Не выбрано подразделение для удаления";
            return RedirectToAction("IndexDepartment", "AdminDepartment");
        }
        public IActionResult EditDepartment(int DepartmentID)
        {
            if (DepartmentID == 0)
            {
                TempData["Info"] = "Не выбрано подразделение для редактирования";
                return RedirectToAction("IndexDepartment", "AdminDepartment");
            }    
            return View("CardDepartment", department.GetOnlyDepartmentID(DepartmentID));
        }

    }
}
