using Plan_current_repairs.Data.Models;
using Plan_current_repairs.Data.VIewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan_current_repairs.Data.Interface
{
    public interface IJornal
    {
        IEnumerable<Jornal> allRecordsJornal { get; }
        IEnumerable<RecordsByJornal> GetJornalByGroup(IEnumerable<NameOfWorks> Group, int DepartmentID, int YearID, string ConditionForSelectionAct, bool FactValue, bool AllDepartment);
        IEnumerable<OtherJornalByFact> GetOtherJornalByFacts(int DepartmentID, int YearID, string ConditionForSelectionAct);
        void EditFactValue(Jornal record, float [] array, float[] fact_parameter_1, float[] fact_parameter_2, float[] fact_parameter_3, string note, string sector);
        void EditPlanValue(Jornal record, float[] array, float[] plan_parameter_1, float[] plan_parameter_2, float[] plan_parameter_3, string note, string sector);
        void AddRecordsToJornal(int departmentID, int yearID, int workID, bool IsPlan);
        void DeleteRecordsFromJornal(int IDRecords);
        IEnumerable<OtherWork> allRecordsJornalOtherWork { get; }
        void AddRecordsOtherWork(OtherWork otherWork);

        void EditOtherFactValue(OtherWork otherWork, float [] array, string note);
        Jornal GetOnlyRecordByID(int IDRecord);
        OtherWork GetOnlyRecordOtherWorkByID(int OtherWorkID);
        void AddActToJornalRecord(int recordID, int otherWorkID, ActOfWork actOfWork);
        void DeleteRecordOtherWork(int OtherWorkID);
    }
}
