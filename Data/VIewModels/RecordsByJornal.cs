using Plan_current_repairs.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan_current_repairs.Data.VIewModels
{
    public class RecordsByJornal
    {
        public NameOfWorks nameOfWorks { get; set; }
        public Jornal jornal { get; set; }
        public float[] FactArray { get; set; }
        public float[] PlanArray { get; set; }
        public float[] FactParameters_1 { get; set; }
        public float[] PlanParameters_1 { get; set; }
        public float[] FactParameters_2 { get; set; }
        public float[] PlanParameters_2 { get; set; }
        public float[] FactParameters_3 { get; set; }
        public float[] PlanParameters_3 { get; set; }

    }
}
