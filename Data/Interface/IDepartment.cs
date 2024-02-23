using Plan_current_repairs.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_current_repairs.Data.Interface
{
    public interface IDepartment
    {
        /// <summary>
        /// Возврат всех филиалов
        /// </summary>
        IEnumerable<Department> allDepartment { get; }
        IEnumerable<Department> allDepartmentWithoutAdministration{ get; }
        IEnumerable<Department> allDepartmentWithEmployee { get; }
        /// <summary>
        /// Добавление или обновление филиала
        /// </summary>
        /// <param name="department"></param>
        void AddAndEditDepartment(Department department);
        /// <summary>
        /// Удаление филиала
        /// </summary>
        /// <param name="DepartmentID"></param>
        void DeleteDepartment(int DepartmentID);
        /// <summary>
        /// Возврат филиала по ID
        /// </summary>
        /// <param name="DepartmentID"></param>
        /// <returns></returns>
        Department GetOnlyDepartmentID(int DepartmentID);
        IEnumerable<Department> GetOnlyDepartmentByUser(Employee employee);
        int GetOnlyDepartmentByName(string name);
       



    }
}
