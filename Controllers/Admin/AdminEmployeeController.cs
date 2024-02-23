using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Plan_current_repairs.Data.Interface;
using Plan_current_repairs.Data.Models;
using Plan_current_repairs.Data.VIewModels;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace Plan_current_repairs.Controllers
{
    //администрирование сотрудников
    public class AdminEmployeeController : Controller
    {
        public readonly IDepartment department;
        public readonly IEmployee employee;
        public readonly ILogger<AdminEmployeeController> logger;
        public AdminEmployeeController(IDepartment department, IEmployee employee, ILogger<AdminEmployeeController> _logger)
        {
            this.department = department;
            this.employee = employee;
            logger = _logger;
        }
        [Authorize("IsAdmin")]
        public IActionResult IndexEmployee(SortStateEmployee sort = SortStateEmployee.FullNameAsc)
        {
            IQueryable<Employee> employees = (IQueryable<Employee>)employee.GetAllEmployee;

            ViewData["FullNameSort"] = sort == SortStateEmployee.FullNameAsc ? SortStateEmployee.FullNameDesc : SortStateEmployee.FullNameAsc;
            ViewData["DepartmentSort"] = sort == SortStateEmployee.DepartmentAcs ? SortStateEmployee.DepartmentDesc : SortStateEmployee.DepartmentAcs;
            ViewData["RoleSort"] = sort == SortStateEmployee.RoleAsc ? SortStateEmployee.RoleDesc : SortStateEmployee.RoleAsc;

            employees = sort switch
            {
                SortStateEmployee.FullNameAsc => employees.OrderBy(x => x.FullName),
                SortStateEmployee.FullNameDesc => employees.OrderByDescending(x => x.FullName),
                SortStateEmployee.DepartmentAcs => employees.OrderBy(x => x.Department),
                SortStateEmployee.DepartmentDesc => employees.OrderByDescending(x => x.Department),
                SortStateEmployee.RoleAsc => employees.OrderBy(x => x.Role),
                SortStateEmployee.RoleDesc => employees.OrderByDescending(x => x.Role),
            };
            
            return View(employees.AsNoTracking());
        }

        [Authorize]
        public IActionResult ListEmployee()
        {
            int departmentId = employee.GetOnlyEmployee(HttpContext.User.Identity.Name).DepartmentID;
            IEnumerable<Employee> employees = employee.GetListAllEmployeeByDepartment(departmentId).OrderBy(x=>x.FullName);
            return View(employees);
        }

        [Authorize]
        [HttpPost]
        public IActionResult ListEmployee(IEnumerable<Employee> employees)
        {
            employee.EditListEmployee(employees);
            return RedirectToAction("ListEmployee", "AdminEmployee");
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddEmployee (string fullName, string post)
        {
            int departmentId = employee.GetOnlyEmployee(HttpContext.User.Identity.Name).DepartmentID;
            employee.AddEmployeeWithoutAccount(fullName, post, departmentId);
            return RedirectToAction("ListEmployee", "AdminEmployee");
        }

        [Authorize]
        public IActionResult DeleteEmployeeWithoutAccount(int EmployeeID)
        {
            if (EmployeeID != 0)
            {
                if(employee.GetOnlyEmployeeID(EmployeeID).Account=="none")
                {
                    TempData["Info"] = $"{employee.GetOnlyEmployeeID(EmployeeID).FullName} удалено";
                    employee.DeleteEmployee(EmployeeID);
                }
            }
            else TempData["Info"] = "Не выбран пользователь для удаления";
            return RedirectToAction("ListEmployee", "AdminEmployee");
        }

        [Authorize("IsAdmin")]
        public IActionResult CardEmployee()
        {
            ViewBag.Departmens = new SelectList(department.allDepartment, "ID", "NameDepartment");
            return View();
        }

        [Authorize("IsAdmin")]
        [HttpPost]
        public IActionResult CardEmployee(Employee employee)
        {
            if (employee.DepartmentID == 0)
            {
                ModelState.AddModelError("", "Выберите филиал");
                if(employee.ID!=0) ViewBag.Action = "Добавить";
                ViewBag.Departmens = new SelectList(department.allDepartment, "ID", "NameDepartment");
                return View("CardEmployee", employee);
               
            }
                this.employee.AddAndEditEmployee(employee, HttpContext.User.Identity.Name);
            return RedirectToAction("IndexEmployee", "AdminEmployee");
        }

        [Authorize("IsAdmin")]
        public IActionResult DeleteEmployee(int EmployeeID)
        {
            if (EmployeeID != 0)
            {
                TempData["Info"] = $"{employee.GetOnlyEmployeeID(EmployeeID).FullName} удалено";
                object[] temp = new object[] { employee.GetOnlyEmployeeID(EmployeeID).Account, employee.GetAllEmployee.FirstOrDefault(x => x.ID == EmployeeID).Department.NameDepartment, employee.GetOnlyEmployeeID(EmployeeID).ID };
                employee.DeleteEmployee(EmployeeID);
                logger.LogInformation("Пользователь {adminUser} удалил учетную запись: логин:{login}, филиал:{department}, ID:{IDEmployee}", HttpContext.User.Identity.Name, temp[0], temp[1], temp[2]);
            }
            else TempData["Info"] = "Не выбран пользователь для удаления";
            return RedirectToAction("IndexEmployee", "AdminEmployee");
        }

        [Authorize("IsAdmin")]
        public IActionResult EditEmployee(int EmployeeID)
        {
            if (EmployeeID == 0)
            {
                TempData["Info"] = "Не выбран пользователь для редактирования";
                return RedirectToAction("IndexEmployee", "AdminEmployee");
            }
            ViewBag.Departmens = new SelectList(department.allDepartment, "ID", "NameDepartment");
            return View("CardEmployee", employee.GetOnlyEmployeeID(EmployeeID));
        }


        public IActionResult ChangePassword()
        {
            Employee currentEmployee = employee.GetOnlyEmployee(HttpContext.User.Identity.Name);
            return View(new ChangePassword {EmployeeID=currentEmployee.ID, FullName=currentEmployee.FullName});
        }


        [HttpPost]
        public IActionResult ChangePassword(ChangePassword changePassword)
        {
           if(changePassword.Password!=changePassword.ConfirmPassword)
            {
                TempData["Info"] = "Введенные пароли не совпадают";
                return RedirectToAction("ChangePassword", "AdminEmployee");
            }
            Employee currentEmployee = employee.GetOnlyEmployee(HttpContext.User.Identity.Name);
            currentEmployee.Password = changePassword.Password;
            employee.AddAndEditEmployee(currentEmployee);
            return RedirectToAction("Index","Home");
        }
    }
}
