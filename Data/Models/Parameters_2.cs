using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_current_repairs.Data.Models
{
    public class Parameters_2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Parameter_2ID { get; set; }

        public float PlanParameter_2ForJanuary { get; set; }
        public float PlanParameter_2ForFebruary { get; set; }
        public float PlanParameter_2ForMarch { get; set; }
        public float PlanParameter_2ForApril { get; set; }
        public float PlanParameter_2ForMay { get; set; }
        public float PlanParameter_2ForJune { get; set; }
        public float PlanParameter_2ForJuly { get; set; }
        public float PlanParameter_2ForAugust { get; set; }
        public float PlanParameter_2ForSeptember { get; set; }
        public float PlanParameter_2ForOctober { get; set; }
        public float PlanParameter_2ForNovember { get; set; }
        public float PlanParameter_2ForDecember { get; set; }

        public float FactParameter_2ForJanuary { get; set; }
        public float FactParameter_2ForFebruary { get; set; }
        public float FactParameter_2ForMarch { get; set; }
        public float FactParameter_2ForApril { get; set; }
        public float FactParameter_2ForMay { get; set; }
        public float FactParameter_2ForJune { get; set; }
        public float FactParameter_2ForJuly { get; set; }
        public float FactParameter_2ForAugust { get; set; }
        public float FactParameter_2ForSeptember { get; set; }
        public float FactParameter_2ForOctober { get; set; }
        public float FactParameter_2ForNovember { get; set; }
        public float FactParameter_2ForDecember { get; set; }
        public int JornalID { get; set; }
        public Jornal Jornal { get; set; }
    }
}
