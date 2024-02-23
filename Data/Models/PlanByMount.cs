using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_current_repairs.Data.Models
{
    /// <summary>
    /// Плановые значения
    /// </summary>
    public class PlanByMount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PlanID { get; set; }
        
        public float PlanForJanuary { get; set; }
        public float PlanForFebruary { get; set; }
        public float PlanForMarch { get; set; }
        public float PlanForApril { get; set; }
        public float PlanForMay { get; set; }
        public float PlanForJune { get; set; }
        public float PlanForJuly { get; set; }
        public float PlanForAugust { get; set; }
        public float PlanForSeptember { get; set; }
        public float PlanForOctober { get; set; }
        public float PlanForNovember { get; set; }
        public float PlanForDecember { get; set; }
        public int JornalID { get; set; }
        public Jornal Jornal { get; set; }
    }
}
