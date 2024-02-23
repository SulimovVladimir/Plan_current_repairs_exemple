using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Connections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plan_current_repairs.Data.Models
{
    public class ActOfWork
    {
        [Key]
        public int IDAct { get; set; }
        [Required]
        public string NumberAct { get; set; }
        public DateTime DateAct { get; set; }
        public string Employee { get; set; }
        public string TitleWork { get; set; }
        public string ResponsibleForWork_Name { get; set; }
        public string ResponsibleForWork_Post { get; set; }
        public int DepartmentID { get; set; }
        public Department Department { get; set; }
        public int YearID { get; set; }
        public Year Year { get; set; }
        public List<Jornal> Jornals { get; set; } = new List<Jornal>();
        public List<OtherWork> OtherWorks { get; set; } = new List<OtherWork>();
        public bool IsCloseEditAndRemove { get; set; }

        //[NotMapped]
        //public string[] EmployeeList 
        //{
        //    get { return Employee == null ? null : Employee.Split(new char[] {';'}); }
        //    set { Employee = EmployeeListTostring(value); }
        //}

        //[NotMapped]
        //public Dictionary<string, string> EmployeeList
        //{
        //    get { return Employee == null? new Dictionary<string, string>() { { "1", "wse" } } : GetEployeeList(Employee); }
        //    set { Employee = EmployeeListTostring(value); }
        //}

        [NotMapped]
        public List<string[]> EmployeeList
        {
            get { return Employee == null ? null : GetEployeeList(Employee); }
            set { Employee = EmployeeListTostring(value); }
        }

        //public string EmployeeListTostring(Dictionary<string, string> value)
        //{
        //    if (value == null) return null;
        //    string result = null;
        //    foreach(KeyValuePair<string, string> el in value)
        //    {
        //        if(el.Key==null) continue;
        //        if(result!=null) result += ";";
        //        result += el.Key + ":" + el.Value;
        //    }
        //    return result;
        //}
        public string EmployeeListTostring(List<string[]> value)
        {
            if (value == null) return null;
            string result = null;
            foreach (string[] el in value)
            {
                if (el[0] == null) continue;
                if (result != null) result += ";";
                result += el[0] + ":" + el[1];
            }
            return result;
        }

        //public Dictionary<string, string> GetEployeeList(string value) 
        //{
        //    Dictionary<string, string> tempDictionary = new Dictionary<string, string>();
        //    if (value == null) {  tempDictionary.Add("1", "reg");
        //        return tempDictionary; }

        //    string[] temp = value.Split(new char[] { ';' });
        //    foreach (string s in temp) 
        //    {

        //        string[] strings = s.Split(new char[] { ':'});
        //        tempDictionary.Add(strings[0], strings[1]);
        //    }
        //    return tempDictionary;
        //}
        public List<string[]> GetEployeeList(string value)
        {
            List<string[]> tempDictionary=new List<string[]>();
            if (value == null)
            {
                return tempDictionary;
            }

            string[] temp = value.Split(new char[] { ';' });
            foreach (string s in temp)
            {

                string[] strings = s.Split(new char[] { ':' });
                tempDictionary.Add(strings);
            }
            return tempDictionary;
        }
    }
}
