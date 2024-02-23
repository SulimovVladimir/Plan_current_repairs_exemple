using Microsoft.AspNetCore.Mvc.Rendering;
using Plan_current_repairs.Data.Models;
using System.Collections.Generic;

namespace Plan_current_repairs.Data.VIewModels
{
    public class DictionaryModel
    {
        public DictionarySector dictionarySector { get; set; }
        public IEnumerable<DictionarySector> listDictionarySectorsByOnlyDepartment { get; set; }
        public SelectList allDepartment { get; set; }
        public int currentDepartment { get; set; }
    }
}
