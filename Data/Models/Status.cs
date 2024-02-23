using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;

namespace Plan_current_repairs.Data.Models
{
    /// <summary>
    /// Класс модели, содержащий информацию о статусе плана-отчета
    /// </summary>
    public class Status
    {
        [Key]
        public int IDStatus { get; set; }
        public string TitleStatus { get; set; }
        public string AssertionPlan { get; set; }
        public string Assertion_1_CalendarQuarter { get; set; }
        public string Assertion_2_CalendarQuarter { get; set; }
        public string Assertion_3_CalendarQuarter { get; set; }
        public string Assertion_4_CalendarQuarter { get; set; }
        public string Blocking { get; set; }
        public int DepartmentID { get; set; }
        public Department Department { get; set; }
        public int YearID { get; set; }
        public Year Year { get; set; }


        [NotMapped]
        public bool[] AssertionPlan_Array
        {
            get { return AssertionPlan == null ? null : StringToBoolArray(AssertionPlan); }
            set { AssertionPlan = BoolArrayToString(value); }
        }
        [NotMapped]
        public bool[] Assertion_1_CalendarQuarter_Array
        {
            get { return Assertion_1_CalendarQuarter == null ? null : StringToBoolArray(Assertion_1_CalendarQuarter); }
            set { Assertion_1_CalendarQuarter = BoolArrayToString(value); }
        }
        [NotMapped]
        public bool[] Assertion_2_CalendarQuarter_Array
        {
            get { return Assertion_2_CalendarQuarter == null ? null : StringToBoolArray(Assertion_2_CalendarQuarter); }
            set { Assertion_2_CalendarQuarter = BoolArrayToString(value); }
        }
        [NotMapped]
        public bool[] Assertion_3_CalendarQuarter_Array
        {
            get { return Assertion_3_CalendarQuarter == null ? null : StringToBoolArray(Assertion_3_CalendarQuarter); }
            set { Assertion_3_CalendarQuarter = BoolArrayToString(value); }
        }
        [NotMapped]
        public bool[] Assertion_4_CalendarQuarter_Array
        {
            get { return Assertion_4_CalendarQuarter == null ? null : StringToBoolArray(Assertion_4_CalendarQuarter); }
            set { Assertion_4_CalendarQuarter = BoolArrayToString(value); }
        }
       
        [NotMapped]
        public bool[] Blocking_Array
        {
            get { return Blocking == null ? null : StringToBoolArray(Blocking); }
            set { Blocking = BoolArrayToString(value); }
        }


        public Status(int departmentID, int yearID)
        {
            TitleStatus = "Создано";
            DepartmentID = departmentID;
            YearID = yearID;
            AssertionPlan_Array = new bool[] {false,false,false,false};
            Assertion_1_CalendarQuarter_Array = new bool[] { false, false, false, false };
            Assertion_2_CalendarQuarter_Array = new bool[] { false, false, false, false };
            Assertion_3_CalendarQuarter_Array = new bool[] { false, false, false, false };
            Assertion_4_CalendarQuarter_Array = new bool[] { false, false, false, false };
            Blocking_Array= new bool[] { false, false, false, false,false };
        }
     

        bool[] StringToBoolArray (string value)
        {
            bool[] result = new bool[value.Count(x=>x ==',')+1];
            string[] temp = value.Split(new char[] { ',' });
            int increment = 0;
            foreach (string s in temp) 
            {
                result[increment] = Convert.ToBoolean(s);
                increment++;
            }
            return result;
        }

        string BoolArrayToString(bool[] value)
        {
            string result = null;
            for (int i=0; i<value.Length;i++)
            {
                result += value[i].ToString();
                if (i < value.Length - 1) result += ",";
            }
            return result;
        }
    }
}
