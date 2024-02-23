using Plan_current_repairs.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan_current_repairs.Data.Interface
{
    public interface IDictionarySector
    {
        IEnumerable<DictionarySector> getDictionarySectorsOnlyDepartment(int IDDepartment);
        void AddAndEditDictionarySector(DictionarySector dictionarySector);
        void DeleteDictionarySector(int DictionarySectorID);
        DictionarySector GetOnlyDictionarySectorID(int DictionarySectorID);
        DictionarySector GetOnlyDictionarySectorbyDepartmentID(int DepartmentID);
    }
}
