using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan_current_repairs.Data.Models
{
    public class OtherWork
    {
        [Key]
        public int OtherWorkID { get; set; }
        public string NameOtherWork { get; set; }
        public string DiscriptionOtherWork { get; set; }
        public string PeriodicityOtherWork { get; set; }
        public string UnitOtherWork { get; set; }
        public string NoteOtherWork { get; set; }
        public int DepartmentID { get; set; }
        public Department department { get; set; }
        public int YearID { get; set; }
        public Year year { get; set; }
        public List<ActOfWork> ActOfWorks { get; set; } = new List<ActOfWork>();
        internal string _FactValue { get; set; }
        internal string _PlanValue { get; set; }
        [NotMapped]
        public string[] OtherFactValue
        {
            get { return _FactValue == null ? null : JsonConvert.DeserializeObject<string[]>(_FactValue); }
            set { _FactValue = JsonConvert.SerializeObject(value); }
        }
        [NotMapped]
        public string[] OtherPlanValue
        {
            get { return _PlanValue == null ? null : JsonConvert.DeserializeObject<string[]>(_PlanValue); }
            set { _PlanValue = JsonConvert.SerializeObject(value); }
        }
    }
}
