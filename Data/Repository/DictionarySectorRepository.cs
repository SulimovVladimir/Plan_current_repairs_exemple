using Microsoft.EntityFrameworkCore;
using Plan_current_repairs.Data.Interface;
using Plan_current_repairs.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan_current_repairs.Data.Repository
{
    public class DictionarySectorRepository : IDictionarySector
    {
        private readonly AppDBContext appDB;
        public DictionarySectorRepository(AppDBContext dBContext)
        {
            appDB = dBContext;
        }
        public void AddAndEditDictionarySector(DictionarySector dictionarySector)
        {
            if (!appDB.DictionarySector.Any(c => c.DictionarySectorID == dictionarySector.DictionarySectorID))
                appDB.DictionarySector.Add(dictionarySector);
            else appDB.DictionarySector.Update(dictionarySector);
            appDB.SaveChanges();
        }

        public void DeleteDictionarySector(int DictionarySectorID)
        {
            appDB.DictionarySector.Remove(GetOnlyDictionarySectorID(DictionarySectorID));
            appDB.SaveChanges();
        }
        public DictionarySector GetOnlyDictionarySectorID(int DictionarySectorID)
        {
            return appDB.DictionarySector.FirstOrDefault(c => c.DictionarySectorID == DictionarySectorID);
        }

        public IEnumerable<DictionarySector> getDictionarySectorsOnlyDepartment(int IDDepartment)
        {
            return appDB.DictionarySector.Where(x => x.DepartmentID == IDDepartment);
        }

        public DictionarySector GetOnlyDictionarySectorbyDepartmentID(int DepartmentID)
        {
            return appDB.DictionarySector.Include(x => x.Department).FirstOrDefault(c => c.DepartmentID == DepartmentID);
        }
    }
}
