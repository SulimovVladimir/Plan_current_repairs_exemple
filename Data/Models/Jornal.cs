using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_current_repairs.Data.Models
{
    /// <summary>
    /// Журнал записей по цеху и году
    /// </summary>
    public class Jornal
    {
        [Key]
        public int NumberRecordingID { get; set; }
        public string Sector { get; set; }
        public string Note { get; set; }
        public int NameOfWorksID { get; set; }
        public NameOfWorks nameOfWorks { get; set; }
        public int DepartmentID { get; set; }
        public Department department { get; set; }
        public int YearID { get; set; }
        public Year year { get; set; }
        public FactByMount FactValue { get; set; } 
        public PlanByMount PlanValue { get; set; }
        public Parameters_1 Parameters_1 { get; set; }
        public Parameters_2 Parameters_2 { get; set; }
        public Parameters_3 Parameters_3 { get; set; }
        public bool CreatedInPlan { get; set; }
        public List<ActOfWork> ActOfWorks { get; set; } = new List<ActOfWork>();
    }
}
