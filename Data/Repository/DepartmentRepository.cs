using Microsoft.EntityFrameworkCore;
using Plan_current_repairs.Data.Interface;
using Plan_current_repairs.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan_current_repairs.Data.Repository
{
    public class DepartmentRepository : IDepartment
    {
        private readonly AppDBContext appDB;
        public DepartmentRepository(AppDBContext dBContext)
        {
            appDB = dBContext;
        }
        public IEnumerable<Department> allDepartment => appDB.Departments;
        public IEnumerable<Department> allDepartmentWithoutAdministration => appDB.Departments.Where(x => x.ID != 1);
        public IEnumerable<Department> allDepartmentWithEmployee => appDB.Departments.Include(x=>x.Employees);

        public IEnumerable<Department> GetOnlyDepartmentByUser(Employee employee)
        {
            return allDepartment.Where(x => x.Employees.Contains(employee));
        }

        public void AddAndEditDepartment(Department department)
        {
            if (!appDB.Departments.Any(c => c.ID == department.ID))
            {
                appDB.Departments.Add(department);
                appDB.SaveChanges();
                AddNullValueJornal(department.ID);
            }
            else
            {
                appDB.Departments.Update(department);
                appDB.SaveChanges();
            }
        }

        public void DeleteDepartment(int departmentID)
        {
            appDB.Departments.Remove(GetOnlyDepartmentID(departmentID));
            appDB.SaveChanges();
        }

       
        public Department GetOnlyDepartmentID(int departmentID)
        {
            return appDB.Departments.FirstOrDefault(c => c.ID == departmentID);
        }

        public int GetOnlyDepartmentByName(string name)
        {
            return appDB.Departments.FirstOrDefault(c => c.NameDepartment == name).ID;
        }

        void AddNullValueJornal(int departmentID)
        {
            List<int> AllYears = new List<int>();
            foreach (var el in appDB.Years) AllYears.Add(el.YearID);

            List<int> AllNameOfWorks = new List<int>();
            foreach (var el in appDB.Works) if (el.TypeRecords == "без участков") AllNameOfWorks.Add(el.WorkID);

            foreach (var year in AllYears)
            {
                foreach (var work in AllNameOfWorks)
                {
                    Jornal recording = new Jornal { YearID = year, NameOfWorksID = work, DepartmentID = departmentID };
                    appDB.Jornals.Add(recording);
                    appDB.SaveChanges();
                    PlanByMount PlanRecording = new PlanByMount { PlanID = recording.NumberRecordingID, Jornal = recording };
                    FactByMount FactRecording = new FactByMount { FactID = recording.NumberRecordingID, Jornal = recording };
                    appDB.PlanValue.Add(PlanRecording);
                    appDB.FactValue.Add(FactRecording);
                    appDB.SaveChanges();
                }
                appDB.Status.Add(new Status(departmentID, year));
                appDB.SaveChanges();
            }
        }


    }
}
