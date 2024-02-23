using Plan_current_repairs.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan_current_repairs.Data.Interface
{
    public interface IYears
    {
        IEnumerable<Year> GetAllYears { get; }
        void AddAndEditYear(Year year);
        int GetIDYearByName(string name);
        Year GetOnlyYearByID(int YearID);
        bool IsCorrectActByNumber(string number, int YearID);
    }
}
