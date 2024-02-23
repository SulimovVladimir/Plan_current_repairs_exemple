using Plan_current_repairs.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan_current_repairs.Data.Interface
{
    public interface IEmployee
    {
        /// <summary>
        /// Создание списка всех сотрудников
        /// </summary>
        IEnumerable<Employee> GetAllEmployee {get;}
        /// <summary>
        /// Возврат одного сотрудника
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Employee GetOnlyEmployee(string userName);
        /// <summary>
        /// проверка на права администратора
        /// </summary>
        /// <param name="fullname"></param>
        /// <returns></returns>
        bool AdminIsTrue(string fullname);
        /// <summary>
        /// Добавление и редактирование сотрудников в БД
        /// </summary>
        /// <param name="employee"></param>
        void AddAndEditEmployee(Employee employee, string adminAccount=null);
        /// <summary>
        /// Удаление сотрудника
        /// </summary>
        /// <param name="EmployeeID"></param>
        void DeleteEmployee(int EmployeeID);
        /// <summary>
        /// Возвращает одного сотрудника по ID
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        Employee GetOnlyEmployeeID(int employeeID);
        public Employee CheckEmployeeLogin(string account, string password);
        List<string> GetListEmailForHeadOfDepartment();
        string GetEmailForFullName(string fullName);
        Dictionary<string, string> GetAllEmployeeByDepartment(int departmentId);
        IEnumerable<Employee> GetListAllEmployeeByDepartment(int departmentId);
        void EditListEmployee(IEnumerable<Employee> employees);
        void AddEmployeeWithoutAccount(string fullName, string post, int departmentID);
    }
}
