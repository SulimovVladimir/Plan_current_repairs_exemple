using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan_current_repairs.Data.Models
{
    public class DictionarySector
    {
        public int DictionarySectorID { get; set; }
        public string Value { get; set; }
        public int DepartmentID { get; set; }
        public Department Department { get; set; }
    }
}
