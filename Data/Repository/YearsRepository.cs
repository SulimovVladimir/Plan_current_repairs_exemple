using Plan_current_repairs.Data.Interface;
using Plan_current_repairs.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan_current_repairs.Data.Repository
{
    public class YearsRepository : IYears
    {
        public readonly AppDBContext appDB;
        public YearsRepository(AppDBContext appDB)
        {
            this.appDB = appDB;
        }
        public IEnumerable<Year> GetAllYears => appDB.Years;
       
        public void AddAndEditYear(Year year)
        {
            if (!appDB.Years.Any(c => c.Years == year.Years))
            {
                appDB.Years.Add(year);
                appDB.SaveChanges();
                AddNullValueJornal(year.YearID);
            }
            else
            {
                if(appDB.Years.Any(c=>c.YearID==year.YearID))
                {appDB.Years.Update(year);
                appDB.SaveChanges(); }
            }
        }
      
        //добавить нулевый значения записям при создании года
        void AddNullValueJornal(int yearID)
        {
            List<int> AllDepartment = new List<int>();
            foreach (var el in appDB.Departments) AllDepartment.Add(el.ID);
            
            List<int> AllNameOfWorks = new List<int>();
            foreach (var el in appDB.Works) if(el.TypeRecords=="без участков")AllNameOfWorks.Add(el.WorkID);

            foreach (var departments in AllDepartment)
            {
                foreach (var work in AllNameOfWorks)
                {
                    Jornal recording = new Jornal { YearID = yearID, NameOfWorksID=work , DepartmentID=departments};
                    appDB.Jornals.Add(recording);
                    appDB.SaveChanges();
                    PlanByMount PlanRecording = new PlanByMount { PlanID=recording.NumberRecordingID, Jornal = recording };
                    FactByMount FactRecording = new FactByMount { FactID=recording.NumberRecordingID, Jornal = recording };
                    appDB.PlanValue.Add(PlanRecording);
                    appDB.FactValue.Add(FactRecording);
                    appDB.SaveChanges();          
                 };
                appDB.Status.Add(new Status(departments, yearID));
                appDB.SaveChanges();
            }

        }

        //получить ID по имени года
        public int GetIDYearByName(string name)
        {
             return appDB.Years.FirstOrDefault(c => c.Years.ToString() == name).YearID; 
                     
        }

        //получить год по ID
        public Year GetOnlyYearByID(int YearID) => appDB.Years.FirstOrDefault(x => x.YearID == YearID);
        
        //проверка на наличие Акта ВР по имени в указанном году
        public bool IsCorrectActByNumber(string number, int YearID) => !appDB.ActsOfWorks.Any(x=>x.NumberAct==number && x.YearID==YearID)?true:false;
       
    }
}
