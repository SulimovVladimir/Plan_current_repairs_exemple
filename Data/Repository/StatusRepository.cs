using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Plan_current_repairs.Data.Interface;
using Plan_current_repairs.Data.Models;
using Plan_current_repairs.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Razor.Parser.SyntaxTree;

namespace Plan_current_repairs.Data.Repository
{
    public class StatusRepository : IStatus
    {
        private readonly AppDBContext appDB;
        private readonly IDepartment department;
        private readonly IEmployee employees;
        private readonly IYears years;
        private readonly IEmailService emailService;
        private readonly IAct acts;
        public readonly Settings settings;
        public StatusRepository(AppDBContext dBContext, IDepartment _department, IYears _years, IEmployee _employees, IEmailService _emailService, IAct _acts, IOptionsMonitor<Settings> _settings)
        {
            appDB = dBContext;
            department = _department;
            years = _years;
            employees = _employees;
            emailService = _emailService;
            acts = _acts;
            settings = _settings.CurrentValue;
        }

        public void AddAndEditStatus(Status status)
        {
            if (!appDB.Status.Any(c => c.IDStatus == status.IDStatus))
            {
                appDB.Status.Add(status);
                appDB.SaveChanges();
            }
            else
            {
                appDB.Status.Update(status);
                appDB.SaveChanges();
            }
        }
        public Status OnlyStatusRecord(string department, string year) 
            => appDB.Status.FirstOrDefault(x => x.DepartmentID == this.department.GetOnlyDepartmentByName(department)  && x.YearID == years.GetIDYearByName(year));
        public Status OnlyStatusRecord(int department, int year)
           => appDB.Status.FirstOrDefault(x => x.DepartmentID == department && x.YearID == year);
        public IEnumerable<Status> ListStatusByNameYear(int year)=> appDB.Status.Where(x => x.YearID == years.GetIDYearByName(year.ToString()) && x.DepartmentID!=1).Include(y=>y.Department).Include(z=>z.Year);
        public IEnumerable<Status> ListStatusByIDYear(int year) => appDB.Status.Where(x => x.YearID == year && x.DepartmentID != 1).Include(y => y.Department).Include(z => z.Year);
        public Status OnlyStatusRecordByID(int id) => appDB.Status.Include(x=>x.Year).FirstOrDefault(x => x.IDStatus == id);

        public void ChangeBlocking(int ID,string userName, string block)
        {
            Status statusValue = OnlyStatusRecordByID(ID);
            Employee employeeIdentity = employees.GetAllEmployee.FirstOrDefault(x => x.Account == userName);
            int index = 10;
            if (employeeIdentity.Department.DirectorDepartment == employeeIdentity.FullName && employeeIdentity.DepartmentID == statusValue.DepartmentID) index = 0;
            if (employeeIdentity.FullName == settings.EngineerOfPTO_Name) index = 1;
            if (employeeIdentity.FullName == settings.HeadOfPTO_Name) index = 2;
            if (employeeIdentity.FullName == settings.ChiefEngineer_Name) index = 3;

            switch (block)
            {
                case "План":
                    statusValue.AssertionPlan = ChangeStringBlocking(statusValue.AssertionPlan, index);
                    if (index == 1) statusValue.Blocking = ChangeStringBlocking(statusValue.Blocking , 0);
                    break;
                case "1-й квартал":
                    statusValue.Assertion_1_CalendarQuarter = ChangeStringBlocking(statusValue.Assertion_1_CalendarQuarter, index);
                    if (index == 1) { statusValue.Blocking = ChangeStringBlocking(statusValue.Blocking, 1); acts.BlockingActs(statusValue.DepartmentID, statusValue.YearID, "-I-"); }
                    break;
                case "2-й квартал":
                    statusValue.Assertion_2_CalendarQuarter = ChangeStringBlocking(statusValue.Assertion_2_CalendarQuarter, index);
                    if (index == 1) { statusValue.Blocking = ChangeStringBlocking(statusValue.Blocking, 2); acts.BlockingActs(statusValue.DepartmentID, statusValue.YearID, "-II-"); }
                        break;
                case "3-й квартал":
                    statusValue.Assertion_3_CalendarQuarter = ChangeStringBlocking(statusValue.Assertion_3_CalendarQuarter, index);
                    if (index == 1) { statusValue.Blocking = ChangeStringBlocking(statusValue.Blocking, 3); acts.BlockingActs(statusValue.DepartmentID, statusValue.YearID, "-III-"); }
                        break;
                case "4-й квартал":
                    statusValue.Assertion_4_CalendarQuarter = ChangeStringBlocking(statusValue.Assertion_4_CalendarQuarter, index);
                    if (index == 1) { statusValue.Blocking = ChangeStringBlocking(statusValue.Blocking, 4); acts.BlockingActs(statusValue.DepartmentID, statusValue.YearID, "-IV-"); }
                        break;
            }
            if (index == 1)
            {
               if(FullAssertion(statusValue.YearID, index, block))
                {
                    string HeadOfPTOEmail = employees.GetEmailForFullName(settings.HeadOfPTO_Name) != null ? employees.GetEmailForFullName(settings.HeadOfPTO_Name) : "VP.Sulimov@samaratransgaz.gazprom.ru";
                    emailService.SendEmailAsync(HeadOfPTOEmail, "План-отчет по текущему ремонту", "Необходимо Ваше согласование. http://stg-usv-web03/Plan_current_repairs.com/");
                }
                
            }
            if (index == 2)
            {
                if (FullAssertion(statusValue.YearID, index, block))
                {
                    string ChiefEngineerEmail = employees.GetEmailForFullName(settings.ChiefEngineer_Name) != null ? employees.GetEmailForFullName(settings.ChiefEngineer_Name) : "VP.Sulimov@samaratransgaz.gazprom.ru";
                    emailService.SendEmailAsync(ChiefEngineerEmail, "План-отчет по текущему ремонту", "Необходимо Ваше утверждение. http://stg-usv-web03/Plan_current_repairs.com/");
                }
            }

            AddAndEditStatus(statusValue);
        }

        string ChangeStringBlocking (string value, int index)
        {
            string[] temp = value.Split(new char[] { ',' });
            temp[index] = "true";
            string result = null;
            for (int i = 0; i < temp.Length; i++)
            {
                result += temp[i].ToString();
                if (i < temp.Length - 1) result += ",";
            }
            return result;
        }

        //Проверка на согласование всех филиалов за отчетный период, для разовой отправки электронного сообщения следуещему утверждающему
        private bool FullAssertion(int year, int index, string block)
        {
            IEnumerable<Status> listStatus = ListStatusByIDYear(year);
            int countAssertion = 0;
            foreach (Status status in listStatus)
            {
                switch (block)
                {
                    case "План":
                        if (status.AssertionPlan_Array[index]) countAssertion++;
                        break;

                    case "1-й квартал":
                        if (status.Assertion_1_CalendarQuarter_Array[index]) countAssertion++;
                        break;

                    case "2-й квартал":
                        if (status.Assertion_2_CalendarQuarter_Array[index]) countAssertion++;
                        break;

                    case "3-й квартал":
                        if (status.Assertion_3_CalendarQuarter_Array[index]) countAssertion++;
                        break;

                    case "4-й квартал":
                        if (status.Assertion_4_CalendarQuarter_Array[index]) countAssertion++;
                        break;
                }
            }

            if (countAssertion <= department.allDepartmentWithoutAdministration.Count()) return true;
            return false;
        }
    }
}
