using Plan_current_repairs.Data.Models;
using System.Collections.Generic;

namespace Plan_current_repairs.Data.Interface
{
    public interface IStatus
    {
        void AddAndEditStatus(Status status);
        Status OnlyStatusRecord(string department, string year);
        Status OnlyStatusRecord(int department, int year);
        IEnumerable <Status> ListStatusByNameYear(int year);
        IEnumerable<Status> ListStatusByIDYear(int year);
        Status OnlyStatusRecordByID(int id);
        void ChangeBlocking(int ID, string userName, string block);
    }
}
