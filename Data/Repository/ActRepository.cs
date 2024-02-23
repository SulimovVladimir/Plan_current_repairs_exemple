using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Plan_current_repairs.Data.Interface;
using Plan_current_repairs.Data.Models;
using Plan_current_repairs.Data.VIewModels;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_current_repairs.Data.Repository
{
    public class ActRepository : IAct
    {
        private readonly AppDBContext appDB;
        public readonly IJornal jornals;
        public readonly Settings settings;

        public ActRepository(AppDBContext dBContext, IJornal _jornals, IOptionsMonitor<Settings> _settings)
        {
            appDB = dBContext;
            jornals = _jornals;
            settings = _settings.CurrentValue;
        }
        public void AddAndEditAct(ActOfWork actOfWork, int RecordID, int OtherRecordID)
        {
            if (!appDB.ActsOfWorks.Any(c => c.IDAct == actOfWork.IDAct))
            {
                appDB.ActsOfWorks.Add(actOfWork);
            }
            else
            {
                if (appDB.ActsOfWorks.Any(x => x.IDAct == actOfWork.IDAct))
                appDB.ActsOfWorks.Update(actOfWork);
            }
            //сопоставление связи многие-ко-многим
            if(RecordID!=0) actOfWork.Jornals.Add(jornals.GetOnlyRecordByID(RecordID));
            if(OtherRecordID != 0) actOfWork.OtherWorks.Add(jornals.GetOnlyRecordOtherWorkByID(OtherRecordID));
            appDB.SaveChanges();
        }
        public void DeleteAct(ActOfWork actOfWork)
        {
            appDB.ActsOfWorks.Remove(actOfWork);
            appDB.SaveChanges();
        }

        public ActOfWork GetOnlyActByID(int ActID) => appDB.ActsOfWorks.Include(d=>d.Department).Include(y=>y.Year).FirstOrDefault(x => x.IDAct == ActID);

        public IEnumerable<ActOfWork> ListActForOnlyDepartment(int departmentID,int yearID, string ConditionForSelectionAct)=>appDB.ActsOfWorks.Where(x => x.DepartmentID == departmentID && x.YearID == yearID && x.NumberAct.Contains(ConditionForSelectionAct));
        public void BlockingActs(int departmentID, int yearID, string conditionForSelectionAct)
        {
            IEnumerable<ActOfWork> listActs = ListActForOnlyDepartment(departmentID, yearID, conditionForSelectionAct);
            foreach(ActOfWork act in listActs) 
            {
                act.IsCloseEditAndRemove = true;
            }
        }

        //открепить Акт ВР от записи в журнале
        public void UnpinCardActFromRecord(int ActID, int RecordID, int OtherRecordID)
        {
            if (RecordID != 0)
            {
                ActOfWork actOfWork = appDB.ActsOfWorks.Include(x => x.Jornals).FirstOrDefault(c => c.IDAct == ActID);
                actOfWork.Jornals.Remove(jornals.GetOnlyRecordByID(RecordID));
            }
            if (OtherRecordID != 0)
            {
                ActOfWork actOfWork = appDB.ActsOfWorks.Include(x => x.OtherWorks).FirstOrDefault(c => c.IDAct == ActID);
                actOfWork.OtherWorks.Remove(jornals.GetOnlyRecordOtherWorkByID(OtherRecordID));
            }
            appDB.SaveChanges();
        }
    }

   
}
