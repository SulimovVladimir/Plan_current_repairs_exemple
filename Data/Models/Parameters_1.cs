using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_current_repairs.Data.Models
{
    public class Parameters_1
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Parameter_1ID { get; set; }
       
        public float PlanParameter_1ForJanuary { get; set; }
        public float PlanParameter_1ForFebruary { get; set; }
        public float PlanParameter_1ForMarch { get; set; }
        public float PlanParameter_1ForApril { get; set; }
        public float PlanParameter_1ForMay { get; set; }
        public float PlanParameter_1ForJune { get; set; }
        public float PlanParameter_1ForJuly { get; set; }
        public float PlanParameter_1ForAugust { get; set; }
        public float PlanParameter_1ForSeptember { get; set; }
        public float PlanParameter_1ForOctober { get; set; }
        public float PlanParameter_1ForNovember { get; set; }
        public float PlanParameter_1ForDecember { get; set; }

        public float FactParameter_1ForJanuary { get; set; }
        public float FactParameter_1ForFebruary { get; set; }
        public float FactParameter_1ForMarch { get; set; }
        public float FactParameter_1ForApril { get; set; }
        public float FactParameter_1ForMay { get; set; }
        public float FactParameter_1ForJune { get; set; }
        public float FactParameter_1ForJuly { get; set; }
        public float FactParameter_1ForAugust { get; set; }
        public float FactParameter_1ForSeptember { get; set; }
        public float FactParameter_1ForOctober { get; set; }
        public float FactParameter_1ForNovember { get; set; }
        public float FactParameter_1ForDecember { get; set; }
        public int JornalID { get; set; }
        public Jornal Jornal { get; set; }
    }
}
