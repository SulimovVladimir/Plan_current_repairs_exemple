using Plan_current_repairs.Data.Models;
using System.Collections.Generic;

namespace Plan_current_repairs.Data.Interface
{
    public interface IAct
    {
        void AddAndEditAct(ActOfWork actOfWork, int RecordID, int OtherRecordID);
        ActOfWork GetOnlyActByID(int ActID);
        IEnumerable<ActOfWork> ListActForOnlyDepartment(int departmentID,int yearID, string ConditionForSelectionAct);
        void DeleteAct(ActOfWork actOfWork);
        void BlockingActs(int departmentID, int yearID, string conditionForSelectionAct);
        void UnpinCardActFromRecord(int ActID, int RecordID, int OtherRecordID);
    }
}
