using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Plan_current_repairs.Data.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Plan_current_repairs.Data.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Plan_current_repairs.Extensions;
using System;
using System.Collections;
using System.IO;
using Org.BouncyCastle.Utilities;

namespace Plan_current_repairs.Controllers
{
    public class HomeController :Controller
    {
        public readonly IEmployee employee;
        public readonly IStatus status;
        public readonly Settings settings;
        public HomeController(IEmployee _employee, IStatus _status, IOptionsMonitor<Settings> _settings)
        {
            employee = _employee;
            status = _status;
            settings = _settings.CurrentValue;
        }
        
       //стартовая страница
        public IActionResult Index()
        {
            if(employee.GetAllEmployee.Count()==0)  //создать учетную запись администратора, если нет пользователей
            {
                employee.AddAndEditEmployee(new Employee {FullName="Aдминистратор",Post="Встроенный администратор системы", Account="Admin", Role="Администратор", DepartmentID=1, Password="12345"});
            }
            IEnumerable<Status> currentStatus = null;
             
            //проверка на аутентификацию
            if (employee.GetAllEmployee.Any(x=>x.Account==HttpContext.User.Identity.Name))
            {
                Employee employeeIdentity = employee.GetAllEmployee.FirstOrDefault(x => x.Account == HttpContext.User.Identity.Name);
                if (employeeIdentity.Role == "Пользователь")
                { 
                    ViewBag.welcome = $"{employeeIdentity.FullName}, Ваша роль - Пользователь";
                    currentStatus = status.ListStatusByNameYear(settings.CurrentYear).Where(x => x.DepartmentID == employeeIdentity.DepartmentID); 
                }
                else if (employeeIdentity.Role == "Рецензент")
                { 
                    ViewBag.welcome = $"{employeeIdentity.FullName}, Ваша роль - Рецензент"; 

                    if(employeeIdentity.FullName==settings.ChiefEngineer_Name || employeeIdentity.FullName == settings.HeadOfPTO_Name || employeeIdentity.FullName == settings.EngineerOfPTO_Name)
                        currentStatus = status.ListStatusByNameYear(settings.CurrentYear);
                    else currentStatus = status.ListStatusByNameYear(settings.CurrentYear).Where(x => x.DepartmentID == employeeIdentity.DepartmentID);
                }
                else
                {
                    ViewBag.welcome = $"{employeeIdentity.FullName}, Ваша роль - Администратор";
                    if (employee.GetAllEmployee.Count() == 1) currentStatus = null;
                    else currentStatus = status.ListStatusByNameYear(settings.CurrentYear);
                }
            }
            else ViewBag.welcome = "Вы зашли как назарегистрированный пользователь";
            return View(currentStatus);
        }

        public IActionResult Login()
        {
            return View();
        }
        //Аутентификация и авторизация пользователя с добавлением Claim(утверждений)
        [HttpPost]
        public async Task<IActionResult> Login(string? returnUrl)
        {
            var form = HttpContext.Request.Form;
            string account = form["account"];
            string password = form["password"];

            Employee employeeIdentity = employee.CheckEmployeeLogin(account,password);
            if (employeeIdentity == null)
            {
                TempData["Info"] = "Данный пользователь не найден, проверьте введенные данные";
                return RedirectToAction("Login");
            }
            var claims = new List<Claim> 
            { 
                new Claim(ClaimTypes.Name, employeeIdentity.Account)
            };
            if (employee.GetAllEmployee.Any(x => x.Account.ToLower() == account.ToLower() && x.Role == "Администратор")) claims.Add(new Claim("Role", "Admin"));
            else if (employee.GetAllEmployee.Any(x => x.Account.ToLower() == account.ToLower() && x.Role == "Пользователь"))
            {
                claims.Add(new Claim("Role", "User"));
                claims.Add(new Claim("Department", employeeIdentity.Department.NameDepartment));
                claims.Add(new Claim("IDDepartment", employeeIdentity.Department.ID.ToString()));
            }
            else
            {
                claims.Add(new Claim("Role", "Reviewer"));
                if (employeeIdentity.Department.DirectorDepartment == employeeIdentity.FullName)
                {
                    claims.Add(new Claim("ReviewerRole", "HeadOfDepartment"));
                    claims.Add(new Claim("Department", employeeIdentity.Department.NameDepartment));
                }
                if (employeeIdentity.FullName == settings.EngineerOfPTO_Name) 
                    claims.Add(new Claim("ReviewerRole", "EngineerOfPTO"));
                if (employeeIdentity.FullName == settings.HeadOfPTO_Name) claims.Add(new Claim("ReviewerRole", "HeadOfPTO"));
                if (employeeIdentity.FullName == settings.ChiefEngineer_Name) claims.Add(new Claim("ReviewerRole", "ChiefEngineer"));
                claims.Add(new Claim("IDDepartment", employeeIdentity.Department.ID.ToString()));
            }


            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            if (password.Equals("12345")) return RedirectToAction("ChangePassword", "AdminEmployee");
            if(returnUrl != null) return Redirect(returnUrl);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Logout(string? returnUrl)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            if (returnUrl != null) return Redirect(returnUrl);
            return RedirectToAction("Index");
        }

        //вывод общих логов
        [Authorize("IsAdmin")]
        public IActionResult ShowLogs()
        {
            List<Log> logs = new List<Log>();
            using (StreamReader sr = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "Logs", "log.txt")))
            {
                string? line;
                while((line = sr.ReadLine())!=null)
                {
                    if(line!="")
                    {
                    string[] strings = line.Split("-----");
                    Log log = new Log() { LogLevel = strings[0], Category= strings[1], Date = Convert.ToDateTime(strings[2]), Message= strings[3]};
                    logs.Add(log);
                    }
                }
            }
            logs.Reverse();
            return View(logs);
        }

        //вывод логов-исключений
        [Authorize("IsAdmin")]
        public IActionResult ShowLogsException()
        {
            string[] logs = System.IO.File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "Logs", "logException.txt"));
            return View(logs);
        }

        public IActionResult ShowHelp()
        {
            return View();
        }

 
    }
}


