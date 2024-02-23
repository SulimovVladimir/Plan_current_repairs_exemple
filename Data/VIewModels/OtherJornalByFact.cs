using Plan_current_repairs.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan_current_repairs.Data.VIewModels
{
    public class OtherJornalByFact
    {
        public OtherWork otherWork { get; set; }
        public float[] OtherFactArray { get; set; }
        public float[] OtherPlanArray { get; set; }
    }
}
