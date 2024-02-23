using Microsoft.AspNetCore.Mvc.Rendering;
using Plan_current_repairs.Data.Interface;
using Plan_current_repairs.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Plan_current_repairs.Data.VIewModels
{
    public class MenuModel

    {

        public SelectList Departments { get; set; }
        public SelectList Years { get; set; }
        public SelectList Sections { get; set; }
        public SelectList Blocks { get; set; }
        public SelectList Groups { get; set; }
        public string CurrentDepartment { get; set; }
        public string CurrentSection { get; set; }
        public string CurrentBlock { get; set; }
        public string CurrentGroup { get; set; }
        public string CurrentYear { get; set; }
        public bool OpenTitle { get; set; }
        public bool CollapseBlock { get; set; }
        public bool ShowNullValue { get; set; }
        
        public bool Master { get; set; }

    }
}
   
