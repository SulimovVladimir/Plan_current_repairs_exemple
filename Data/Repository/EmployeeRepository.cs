using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Plan_current_repairs.Data.Interface;
using Plan_current_repairs.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Plan_current_repairs.Data.Repository
{
    public class EmployeeRepository : IEmployee
    {
        private readonly AppDBContext appDB;
        public readonly Settings settings;
        public readonly ILogger<EmployeeRepository> logger;
        

        public EmployeeRepository(AppDBContext dBContext, IOptionsMonitor<Settings> _settings, ILogger<EmployeeRepository> _logger)
        {
            appDB = dBContext;
            settings = _settings.CurrentValue;
            logger = _logger;
        }
       
        public IEnumerable<Employee> GetAllEmployee => appDB.Employees.Include(p=>p.Department);

        public Employee GetOnlyEmployee(string account) => appDB.Employees.FirstOrDefault(c => c.Account == account);

        public bool AdminIsTrue(string account)
        {
            Employee currentEmployee = appDB.Employees.FirstOrDefault(c => c.Account == account);
            if (currentEmployee != null)
            {
                if(currentEmployee.Role== "Администратор" || currentEmployee.FullName == settings.EngineerOfPTO_Name)
                return true; 
            } 

            return false;

        }

        public void AddAndEditEmployee(Employee employee, string adminAccount = null)
        {
            bool flag=false;
            if (!appDB.Employees.Any(c => c.ID == employee.ID))
            {
                if(employee.Password!=null) employee.Password = ComputeHash(employee.Password);
                appDB.Employees.Add(employee);
                flag=true;
            }
            else 
            {
                if(appDB.Employees.Any(x=>x.ID==employee.ID && x.Password!=employee.Password)) 
                    employee.Password = ComputeHash(employee.Password);
                appDB.Employees.Update(employee);
            }
            appDB.SaveChanges();
            if(flag && adminAccount!=null)logger.LogInformation("Пользователь {adminUser} добавил учетную запись: логин:{login}, филиал:{department}, ID:{IDEmployee}", adminAccount, employee.Account, GetAllEmployee.FirstOrDefault(x=>x.ID==employee.ID).Department.NameDepartment, employee.ID);
        }

        public void DeleteEmployee(int employeeID)
        {
            appDB.Employees.Remove(GetOnlyEmployeeID(employeeID));
            appDB.SaveChanges();
        }

        public Employee GetOnlyEmployeeID(int employeeID)
        {
            return appDB.Employees.FirstOrDefault(c=>c.ID==employeeID);
        }

        public Employee CheckEmployeeLogin(string account, string password)
        {
            return appDB.Employees.FirstOrDefault(x => x.Account.ToLower() == account.ToLower() && x.Password == ComputeHash(password));
        }

        //хэширование пароля
        private string ComputeHash(string pass)
        {
            StringBuilder HashPass = new StringBuilder();
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(pass));
                foreach (byte b in hashValue)
                {
                    HashPass.Append(b);
                }
            }
            return HashPass.ToString();
        }

        //получить список электронных адресов для начальников цехов
        public List<string> GetListEmailForHeadOfDepartment()
        {
            List<string> ListEmailForHeadOfDepartment = new List<string>();
            var listEmployee = appDB.Employees.Where(x => x.FullName == x.Department.DirectorDepartment);
            foreach (var employee in listEmployee) 
            {
                ListEmailForHeadOfDepartment.Add(employee.Email);
            }
            return ListEmailForHeadOfDepartment;
        }

        public string GetEmailForFullName (string fullName)
        {
            Employee employee = appDB.Employees.FirstOrDefault(x => x.FullName == fullName);
            if (employee != null) return employee.Email;
            return null;
        }

        public Dictionary<string,string> GetAllEmployeeByDepartment (int departmentId)
        {
            Dictionary<string, string> listDictionaryEmployee = new Dictionary<string, string>();
            IEnumerable<Employee> listEmployee = GetAllEmployee.Where(x => x.DepartmentID == departmentId);
            foreach (Employee employee in listEmployee) 
            {
                listDictionaryEmployee.Add(employee.FullName, employee.Post);
            }
            return listDictionaryEmployee;
        }
        public IEnumerable<Employee> GetListAllEmployeeByDepartment(int departmentId) => GetAllEmployee.Where(x => x.DepartmentID == departmentId);

        public void EditListEmployee(IEnumerable<Employee> employees)
        {
            foreach (Employee item in employees) 
            {
                Employee tempEmployee = GetOnlyEmployeeID(item.ID);
                if (tempEmployee.Post!=item.Post) 
                {
                    tempEmployee.Post = item.Post;
                    AddAndEditEmployee(tempEmployee);
                }
            }
        }

        //добавление работника без аккаунта
        public void AddEmployeeWithoutAccount(string fullName, string post, int departmentID)
        {
            fullName = System.Text.RegularExpressions.Regex.Replace(fullName, @"\s+", " ").Trim();
            if (!appDB.Employees.Any(x=>x.FullName==fullName && x.DepartmentID==departmentID))
            AddAndEditEmployee(new Employee
            {
                FullName = fullName,
                Post = post,
                DepartmentID = departmentID,
                Role = "Пользователь",
                Account="none"
            });
        }

    }
}
