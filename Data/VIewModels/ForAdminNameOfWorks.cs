using Microsoft.AspNetCore.Mvc.Rendering;
using Plan_current_repairs.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan_current_repairs.Data.VIewModels
{
    public class ForAdminNameOfWorks
    {
        public int WorkID { get; set; }
        public IEnumerable<NameOfWorks> allNameOfWorks;
   
    }
}
