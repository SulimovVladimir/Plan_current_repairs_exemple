using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_current_repairs.Data.Models
{
    /// <summary>
    /// Фактические значения
    /// </summary>
    public class FactByMount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FactID { get; set; }
        public float FactForJanuary { get; set; }
        public float FactForFebruary { get; set; }
        public float FactForMarch { get; set; }
        public float FactForApril { get; set; }
        public float FactForMay { get; set; }
        public float FactForJune { get; set; }
        public float FactForJuly { get; set; }
        public float FactForAugust { get; set; }
        public float FactForSeptember { get; set; }
        public float FactForOctober { get; set; }
        public float FactForNovember { get; set; }
        public float FactForDecember { get; set; }
        public int JornalID { get; set; }
        public Jornal Jornal { get; set; }
    }
}
