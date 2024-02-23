using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Plan_current_repairs.Data.Interface;
using Plan_current_repairs.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan_current_repairs.Data.Repository
{
    public class WorksRepository :IWorks
    {
        public readonly AppDBContext appDB;
        public WorksRepository(AppDBContext appDB)
        {
            this.appDB = appDB;
        }

        //получить все группы работ (по умолчанию созданная группа "Дополнительные работы", перемещаем в конец списка)
        public IEnumerable<GroupNameOfWorks> AllGroupNameOfWorks()
        {
            List<GroupNameOfWorks> tempAllGroups = appDB.Groups.ToList();
            GroupNameOfWorks tempFirstGroup = tempAllGroups[0];
            tempAllGroups.RemoveAt(0);
            tempAllGroups.Add(tempFirstGroup);
            return tempAllGroups;
        }

        public IEnumerable<NameOfWorks> AllNameOfWorks => appDB.Works.Include(c => c.GroupName);

        public void AddAndEditGroupNameofWorks(GroupNameOfWorks groupName)
        {
            if (!appDB.Groups.Any(c => c.GroupID == groupName.GroupID))
                appDB.Groups.Add(groupName);
            else appDB.Groups.Update(groupName);
            appDB.SaveChanges();
        }

        public void AddAndEditNameofWorks(NameOfWorks nameOfWorks)
        {
            if (!appDB.Works.Any(c => c.WorkID == nameOfWorks.WorkID))
            {
                appDB.Works.Add(nameOfWorks);
                appDB.SaveChanges();
                if (nameOfWorks.TypeRecords == "без участков") AddNullValueJornal(nameOfWorks.WorkID);
            }
            else
            { 
                if(nameOfWorks.TypeRecords== "с участками и параметрами")
                {
                    IEnumerable<Jornal> ListRecords = appDB.Jornals.Where(x => x.NameOfWorksID == nameOfWorks.WorkID);
                    List<int> ListRecordsID = ListRecords.Select(x => x.NumberRecordingID).ToList();
                    var parametr_1 = appDB.Parameters_1.Where(x => ListRecordsID.Contains(x.Parameter_1ID));
                    var parametr_2 = appDB.Parameters_2.Where(x => ListRecordsID.Contains(x.Parameter_2ID));
                    var parametr_3 = appDB.Parameters_3.Where(x => ListRecordsID.Contains(x.Parameter_3ID));
                    if (nameOfWorks.Parameter_1 == null) { if (parametr_1 != null) appDB.RemoveRange(parametr_1); }
                    else {
                            foreach (var record in ListRecords)
                            {
                                if (!parametr_1.Any(x => x.Parameter_1ID == record.NumberRecordingID)) appDB.Parameters_1.Add(new Parameters_1 {Parameter_1ID=record.NumberRecordingID, Jornal=record } );
                            }
                         }

                    if (nameOfWorks.Parameter_2 == null) { if (parametr_2 != null) appDB.RemoveRange(parametr_2); }
                    else
                    {
                        foreach (var record in ListRecords)
                        {
                            if (!parametr_2.Any(x => x.Parameter_2ID == record.NumberRecordingID)) appDB.Parameters_2.Add(new Parameters_2 { Parameter_2ID = record.NumberRecordingID, Jornal = record });
                        }
                    }

                    if (nameOfWorks.Parameter_3 == null) { if (parametr_3 != null) appDB.RemoveRange(parametr_3); }
                    else
                    {
                        foreach (var record in ListRecords)
                        {
                            if (!parametr_3.Any(x => x.Parameter_3ID == record.NumberRecordingID)) appDB.Parameters_3.Add(new Parameters_3 { Parameter_3ID = record.NumberRecordingID, Jornal = record });
                        }
                    }
                }
                               
                appDB.Works.Update(nameOfWorks);
                appDB.SaveChanges();
            }
        }

        //получить все работы для одной группы по ID группы
        public IEnumerable<NameOfWorks> GetNameOfWorksOnlyGroup(int groupID)
        {
            return appDB.Works.Where(c => c.GroupNameOfWorksID == groupID);
        }

        //получить все работы для одной группы по имени группы
        public IEnumerable<NameOfWorks> GetNameOfWorksOnlyGroup(string groupName)
        {
            return appDB.Works.Where(c => c.GroupNameOfWorksID == GetGroupId(groupName));
        }

        int GetGroupId (string nameGroup)
        {
            return appDB.Groups.FirstOrDefault(p => p.NameGroup == nameGroup).GroupID;
        }

        void AddNullValueJornal(int workID)
        {
            List<int> AllDepartment = new List<int>();
            foreach (var el in appDB.Departments) if(el.ID!=1)AllDepartment.Add(el.ID);

            List<int> AllYears = new List<int>();
            foreach (var el in appDB.Years) AllYears.Add(el.YearID);

            foreach (var departments in AllDepartment)
            {
                foreach (var year in AllYears)
                {
                    Jornal recording = new Jornal { YearID = year, NameOfWorksID = workID, DepartmentID = departments, CreatedInPlan = true };
                    appDB.Jornals.Add(recording);
                    appDB.SaveChanges();
                    PlanByMount PlanRecording = new PlanByMount { PlanID = recording.NumberRecordingID, Jornal = recording };
                    FactByMount FactRecording = new FactByMount { FactID = recording.NumberRecordingID, Jornal = recording };
                    appDB.PlanValue.Add(PlanRecording);
                    appDB.FactValue.Add(FactRecording);
                    appDB.Status.Add(new Status(departments, year));
                    appDB.SaveChanges();
                }
            }
        }

        public void DeleteNameOfWorks(int workID)
        {
                appDB.Works.Remove(appDB.Works.FirstOrDefault(x => x.WorkID == workID));
                appDB.SaveChanges();
        }

        public void DeleteGroupNameOfWorks(int groupID)
        {
            appDB.Groups.Remove(appDB.Groups.FirstOrDefault(x => x.GroupID == groupID));
            appDB.SaveChanges();
        }

        public NameOfWorks GetOnlyNameByID(int workID)
        {
            return appDB.Works.FirstOrDefault(x => x.WorkID == workID);
        }

        public GroupNameOfWorks GetOnlyGroupByID(int groupID)
        {
            return appDB.Groups.FirstOrDefault(x=>x.GroupID== groupID);
        }

      
        public List<SelectListItem> GetListGroupNameOfWorksForSelect()
        {
            List<SelectListItem> templist = new List<SelectListItem>();
            foreach (var item in AllGroupNameOfWorks())
            {
                SelectListItem el = new SelectListItem
                { Value = item.GroupID.ToString(), Text = item.NameGroup.ToString() };
                templist.Add(el);
            }
            return templist;
        }

        //проверка приложения на типовые ошибки
        // - запись не заполнена нулевыми значениями
        public void CheckJornal()
        {
            IEnumerable<NameOfWorks> AllWorks = appDB.Works.Where(x => x.TypeRecords == "без участков").ToList();
            foreach (var work in AllWorks)
            {
                    if (!appDB.Jornals.Any(x => x.NameOfWorksID == work.WorkID)) 
                        AddNullValueJornal(work.WorkID);
            }
        }
    }
}
