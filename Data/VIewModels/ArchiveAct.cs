using Microsoft.AspNetCore.Mvc.Rendering;
using Plan_current_repairs.Data.Models;
using System.Collections.Generic;

namespace Plan_current_repairs.Data.VIewModels
{
    public class ArchiveAct
    {
        public IEnumerable<ActOfWork> ListAct;
        public SelectList Departments { get; set; }
        public SelectList Years { get; set; }
        public string CurrentDepartment { get; set; }
        public string CurrentYear { get; set; }
        public string CurrentAct { get; set; }
    }
}
