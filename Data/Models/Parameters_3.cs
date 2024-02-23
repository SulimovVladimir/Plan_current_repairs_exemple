using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_current_repairs.Data.Models
{
    public class Parameters_3
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Parameter_3ID { get; set; }

        public float PlanParameter_3ForJanuary { get; set; }
        public float PlanParameter_3ForFebruary { get; set; }
        public float PlanParameter_3ForMarch { get; set; }
        public float PlanParameter_3ForApril { get; set; }
        public float PlanParameter_3ForMay { get; set; }
        public float PlanParameter_3ForJune { get; set; }
        public float PlanParameter_3ForJuly { get; set; }
        public float PlanParameter_3ForAugust { get; set; }
        public float PlanParameter_3ForSeptember { get; set; }
        public float PlanParameter_3ForOctober { get; set; }
        public float PlanParameter_3ForNovember { get; set; }
        public float PlanParameter_3ForDecember { get; set; }

        public float FactParameter_3ForJanuary { get; set; }
        public float FactParameter_3ForFebruary { get; set; }
        public float FactParameter_3ForMarch { get; set; }
        public float FactParameter_3ForApril { get; set; }
        public float FactParameter_3ForMay { get; set; }
        public float FactParameter_3ForJune { get; set; }
        public float FactParameter_3ForJuly { get; set; }
        public float FactParameter_3ForAugust { get; set; }
        public float FactParameter_3ForSeptember { get; set; }
        public float FactParameter_3ForOctober { get; set; }
        public float FactParameter_3ForNovember { get; set; }
        public float FactParameter_3ForDecember { get; set; }
        public int JornalID { get; set; }
        public Jornal Jornal { get; set; }
    }
}
