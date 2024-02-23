using Microsoft.EntityFrameworkCore;
using Plan_current_repairs.Data.Interface;
using Plan_current_repairs.Data.Models;
using Plan_current_repairs.Data.VIewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan_current_repairs.Data.Repository
{
    public class JornalRepository : IJornal
    {
        public readonly AppDBContext appDB;
        public JornalRepository(AppDBContext appDB)
        {
            this.appDB = appDB;
        }

        public IEnumerable<Jornal> allRecordsJornal => appDB.Jornals;
        public IEnumerable<OtherWork> allRecordsJornalOtherWork => appDB.OtherWorks;

        //получить все записи для группы работ
        public IEnumerable<RecordsByJornal> GetJornalByGroup(IEnumerable<NameOfWorks> ListNameOfWorks, int DepartmentID, int YearID,string ConditionForSelectionAct, bool FactValue, bool AllDepartment)
        {

            List<RecordsByJornal> TempGetJornal = new List<RecordsByJornal>();
            
            foreach (var work in ListNameOfWorks.ToList())
            {
                List<Jornal> TempListJornal;

                if (FactValue)
                    {
                        TempListJornal = (AllDepartment) ? new List<Jornal>(appDB.Jornals.Include(a => a.ActOfWorks).Where(c => c.NameOfWorksID == work.WorkID && c.YearID == YearID && c.DepartmentID!=1).Include(i=>i.department)):
                        new List<Jornal>(appDB.Jornals.Include(a => a.ActOfWorks.Where(c=>c.NumberAct.Contains(ConditionForSelectionAct))).Where(c => c.NameOfWorksID == work.WorkID && c.DepartmentID == DepartmentID && c.YearID == YearID)); 
                    }
                else
                {
                    TempListJornal = (AllDepartment) ? new List<Jornal>(appDB.Jornals.Include(a => a.ActOfWorks).Where(c =>c.CreatedInPlan == true && c.NameOfWorksID == work.WorkID && c.YearID == YearID && c.DepartmentID != 1).Include(i => i.department)) :
                        new List<Jornal>(appDB.Jornals.Include(a=>a.ActOfWorks.Where(c => c.NumberAct.Contains(ConditionForSelectionAct))).Where(c => c.NameOfWorksID == work.WorkID && c.DepartmentID == DepartmentID && c.YearID == YearID));
                }

                if (TempListJornal.Count == 0)
                {
                    if(FactValue) TempGetJornal.Add(new RecordsByJornal { nameOfWorks = work, jornal = null, FactArray = null, PlanArray = null,
                        PlanParameters_1 = null, FactParameters_1 = null, FactParameters_2 = null, FactParameters_3 = null, PlanParameters_2 = null, PlanParameters_3 = null });

                    else TempGetJornal.Add(new RecordsByJornal { nameOfWorks = work, jornal = null, PlanArray = null,
                        PlanParameters_1 = null, PlanParameters_2 = null, PlanParameters_3 = null });

                    continue;
                }
                foreach (var TempJornal in TempListJornal)
                {
                    PlanByMount TempPlan = appDB.PlanValue.FirstOrDefault(p => p.JornalID == TempJornal.NumberRecordingID);
                    FactByMount TempFact = appDB.FactValue.FirstOrDefault(f => f.JornalID == TempJornal.NumberRecordingID);
                    Parameters_1 TempParameters_1 = appDB.Parameters_1.FirstOrDefault(a => a.JornalID == TempJornal.NumberRecordingID);
                    Parameters_2 TempParameters_2 = appDB.Parameters_2.FirstOrDefault(b => b.JornalID == TempJornal.NumberRecordingID);
                    Parameters_3 TempParameters_3 = appDB.Parameters_3.FirstOrDefault(c => c.JornalID == TempJornal.NumberRecordingID);

                    float[] TempFactArray = null;
                    if (FactValue) TempFactArray = new float[12] {
                        TempFact.FactForJanuary,
                        TempFact.FactForFebruary,
                        TempFact.FactForMarch,
                        TempFact.FactForApril,
                        TempFact.FactForMay,
                        TempFact.FactForJune,
                        TempFact.FactForJuly,
                        TempFact.FactForAugust,
                        TempFact.FactForSeptember,
                        TempFact.FactForOctober,
                        TempFact.FactForNovember,
                        TempFact.FactForDecember};
                   
                   
                    float[] TempPlanArray = new float[12] {
                    TempPlan.PlanForJanuary,
                    TempPlan.PlanForFebruary,
                    TempPlan.PlanForMarch,
                    TempPlan.PlanForApril,
                    TempPlan.PlanForMay,
                    TempPlan.PlanForJune,
                    TempPlan.PlanForJuly,
                    TempPlan.PlanForAugust,
                    TempPlan.PlanForSeptember,
                    TempPlan.PlanForOctober,
                    TempPlan.PlanForNovember,
                    TempPlan.PlanForDecember
                };

                    float[] TempPlanParameter_1 = null;
                    float[] TempFactParameter_1 = null;
                    float[] TempPlanParameter_2 = null;
                    float[] TempFactParameter_2 = null;
                    float[] TempPlanParameter_3 = null;
                    float[] TempFactParameter_3 = null;

                    if (TempParameters_1 != null)
                    {
                        TempPlanParameter_1 = new float[12]
                        {
                           TempParameters_1.PlanParameter_1ForJanuary,
                           TempParameters_1.PlanParameter_1ForFebruary,
                           TempParameters_1.PlanParameter_1ForMarch,
                           TempParameters_1.PlanParameter_1ForApril,
                           TempParameters_1.PlanParameter_1ForMay,
                           TempParameters_1.PlanParameter_1ForJune,
                           TempParameters_1.PlanParameter_1ForJuly,
                           TempParameters_1.PlanParameter_1ForAugust,
                           TempParameters_1.PlanParameter_1ForSeptember,
                           TempParameters_1.PlanParameter_1ForOctober,
                           TempParameters_1.PlanParameter_1ForNovember,
                           TempParameters_1.PlanParameter_1ForDecember
                        };

                        if(FactValue) TempFactParameter_1 = new float[12]
                       {
                           TempParameters_1.FactParameter_1ForJanuary,
                           TempParameters_1.FactParameter_1ForFebruary,
                           TempParameters_1.FactParameter_1ForMarch,
                           TempParameters_1.FactParameter_1ForApril,
                           TempParameters_1.FactParameter_1ForMay,
                           TempParameters_1.FactParameter_1ForJune,
                           TempParameters_1.FactParameter_1ForJuly,
                           TempParameters_1.FactParameter_1ForAugust,
                           TempParameters_1.FactParameter_1ForSeptember,
                           TempParameters_1.FactParameter_1ForOctober,
                           TempParameters_1.FactParameter_1ForNovember,
                           TempParameters_1.FactParameter_1ForDecember
                       };
                    }
                    if (TempParameters_2 != null)
                    {
                        TempPlanParameter_2 = new float[12]
                      {
                           TempParameters_2.PlanParameter_2ForJanuary,
                           TempParameters_2.PlanParameter_2ForFebruary,
                           TempParameters_2.PlanParameter_2ForMarch,
                           TempParameters_2.PlanParameter_2ForApril,
                           TempParameters_2.PlanParameter_2ForMay,
                           TempParameters_2.PlanParameter_2ForJune,
                           TempParameters_2.PlanParameter_2ForJuly,
                           TempParameters_2.PlanParameter_2ForAugust,
                           TempParameters_2.PlanParameter_2ForSeptember,
                           TempParameters_2.PlanParameter_2ForOctober,
                           TempParameters_2.PlanParameter_2ForNovember,
                           TempParameters_2.PlanParameter_2ForDecember
                      };

                        if(FactValue) TempFactParameter_2 = new float[12]
                       {
                            TempParameters_2.FactParameter_2ForJanuary,
                            TempParameters_2.FactParameter_2ForFebruary,
                            TempParameters_2.FactParameter_2ForMarch,
                            TempParameters_2.FactParameter_2ForApril,
                            TempParameters_2.FactParameter_2ForMay,
                            TempParameters_2.FactParameter_2ForJune,
                            TempParameters_2.FactParameter_2ForJuly,
                           TempParameters_2.FactParameter_2ForAugust,
                           TempParameters_2.FactParameter_2ForSeptember,
                           TempParameters_2.FactParameter_2ForOctober,
                           TempParameters_2.FactParameter_2ForNovember,
                           TempParameters_2.FactParameter_2ForDecember
                       };
                    }
                    if (TempParameters_3 != null)
                    {
                        TempPlanParameter_3 = new float[12]
                          {
                            TempParameters_3.PlanParameter_3ForJanuary,
                            TempParameters_3.PlanParameter_3ForFebruary,
                            TempParameters_3.PlanParameter_3ForMarch,
                            TempParameters_3.PlanParameter_3ForApril,
                            TempParameters_3.PlanParameter_3ForMay,
                            TempParameters_3.PlanParameter_3ForJune,
                            TempParameters_3.PlanParameter_3ForJuly,
                           TempParameters_3.PlanParameter_3ForAugust,
                           TempParameters_3.PlanParameter_3ForSeptember,
                           TempParameters_3.PlanParameter_3ForOctober,
                           TempParameters_3.PlanParameter_3ForNovember,
                           TempParameters_3.PlanParameter_3ForDecember
                          };

                        if(FactValue) TempFactParameter_3 = new float[12]
                       {
                            TempParameters_3.FactParameter_3ForJanuary,
                            TempParameters_3.FactParameter_3ForFebruary,
                            TempParameters_3.FactParameter_3ForMarch,
                            TempParameters_3.FactParameter_3ForApril,
                            TempParameters_3.FactParameter_3ForMay,
                            TempParameters_3.FactParameter_3ForJune,
                            TempParameters_3.FactParameter_3ForJuly,
                           TempParameters_3.FactParameter_3ForAugust,
                           TempParameters_3.FactParameter_3ForSeptember,
                           TempParameters_3.FactParameter_3ForOctober,
                           TempParameters_3.FactParameter_3ForNovember,
                           TempParameters_3.FactParameter_3ForDecember
                       };
                    }

                    if(FactValue) TempGetJornal.Add(new RecordsByJornal { nameOfWorks = work, jornal = TempJornal, FactArray = TempFactArray, PlanArray = TempPlanArray,
                        PlanParameters_1 = TempPlanParameter_1, PlanParameters_2 = TempPlanParameter_2, PlanParameters_3 = TempPlanParameter_3,
                        FactParameters_1 = TempFactParameter_1, FactParameters_2 = TempFactParameter_2, FactParameters_3 = TempFactParameter_3 });

                    else TempGetJornal.Add(new RecordsByJornal  { nameOfWorks = work, jornal = TempJornal, PlanArray = TempPlanArray,
                        PlanParameters_1 = TempPlanParameter_1, PlanParameters_2 = TempPlanParameter_2, PlanParameters_3 = TempPlanParameter_3});
                }
            }
            return TempGetJornal;
        }

        public IEnumerable<OtherJornalByFact> GetOtherJornalByFacts(int DepartmentID, int YearID, string ConditionForSelectionAct)
        {
            List<OtherJornalByFact> tempOtherJornalByFacts = new List<OtherJornalByFact>();
            List<OtherWork> tempOtherWork = new List<OtherWork>(appDB.OtherWorks.Include(a => a.ActOfWorks.Where(c => c.NumberAct.Contains(ConditionForSelectionAct))).Where(x => x.DepartmentID == DepartmentID && x.YearID == YearID));
            foreach (var tempWork in tempOtherWork)
            {
                float[] tempPlan = new float[12];
                float[] tempFact = new float[12];

                for (int i = 0; i <tempPlan.Length; i++)
                {
                    tempPlan[i] = float.Parse(tempWork.OtherPlanValue[i]);
                    tempFact[i] = float.Parse(tempWork.OtherFactValue[i]);
                }
                tempOtherJornalByFacts.Add(new OtherJornalByFact { otherWork = tempWork, OtherFactArray = tempFact, OtherPlanArray = tempPlan });
            }
            return tempOtherJornalByFacts;
        }

        public void EditFactValue(Jornal record, float [] array, float[] fact_parameter_1, float[] fact_parameter_2, float[] fact_parameter_3, string note, string sector)
        {
            FactByMount tempFactByMount = new FactByMount
            {
             
                FactID = record.NumberRecordingID,
                JornalID = record.NumberRecordingID,
                FactForJanuary = array[0],
                FactForFebruary = array[1],
                FactForMarch = array[2],
                FactForApril = array[3],
                FactForMay = array[4],
                FactForJune = array[5],
                FactForJuly = array[6],
                FactForAugust = array[7],
                FactForSeptember = array[8],
                FactForOctober = array[9],
                FactForNovember = array[10],
                FactForDecember = array[11]
            };

            if(fact_parameter_1!=null)
            {
                Parameters_1 TempParameters_1 = new Parameters_1
                {
                    Parameter_1ID = record.NumberRecordingID,
                    JornalID = record.NumberRecordingID,
                    FactParameter_1ForJanuary = fact_parameter_1[0],
                    FactParameter_1ForFebruary = fact_parameter_1[1],
                    FactParameter_1ForMarch = fact_parameter_1[2],
                    FactParameter_1ForApril = fact_parameter_1[3],
                    FactParameter_1ForMay = fact_parameter_1[4],
                    FactParameter_1ForJune = fact_parameter_1[5],
                    FactParameter_1ForJuly = fact_parameter_1[6],
                    FactParameter_1ForAugust = fact_parameter_1[7],
                    FactParameter_1ForSeptember = fact_parameter_1[8],
                    FactParameter_1ForOctober = fact_parameter_1[9],
                    FactParameter_1ForNovember = fact_parameter_1[10],
                    FactParameter_1ForDecember = fact_parameter_1[11]
                };
                appDB.Parameters_1.Update(TempParameters_1);
            }

            if (fact_parameter_2 != null)
            {
                Parameters_2 TempParameters_2 = new Parameters_2
                {
                    Parameter_2ID = record.NumberRecordingID,
                    JornalID = record.NumberRecordingID,
                    FactParameter_2ForJanuary = fact_parameter_2[0],
                    FactParameter_2ForFebruary = fact_parameter_2[1],
                    FactParameter_2ForMarch = fact_parameter_2[2],
                    FactParameter_2ForApril = fact_parameter_2[3],
                    FactParameter_2ForMay = fact_parameter_2[4],
                    FactParameter_2ForJune = fact_parameter_2[5],
                    FactParameter_2ForJuly = fact_parameter_2[6],
                    FactParameter_2ForAugust = fact_parameter_2[7],
                    FactParameter_2ForSeptember = fact_parameter_2[8],
                    FactParameter_2ForOctober = fact_parameter_2[9],
                    FactParameter_2ForNovember = fact_parameter_2[10],
                    FactParameter_2ForDecember = fact_parameter_2[11]
                };
                appDB.Parameters_2.Update(TempParameters_2);
            }

            if (fact_parameter_3 != null)
            {
                Parameters_3 TempParameters_3 = new Parameters_3
                {
                    Parameter_3ID = record.NumberRecordingID,
                    JornalID = record.NumberRecordingID,
                    FactParameter_3ForJanuary = fact_parameter_3[0],
                    FactParameter_3ForFebruary = fact_parameter_3[1],
                    FactParameter_3ForMarch = fact_parameter_3[2],
                    FactParameter_3ForApril = fact_parameter_3[3],
                    FactParameter_3ForMay = fact_parameter_3[4],
                    FactParameter_3ForJune = fact_parameter_3[5],
                    FactParameter_3ForJuly = fact_parameter_3[6],
                    FactParameter_3ForAugust = fact_parameter_3[7],
                    FactParameter_3ForSeptember = fact_parameter_3[8],
                    FactParameter_3ForOctober = fact_parameter_3[9],
                    FactParameter_3ForNovember = fact_parameter_3[10],
                    FactParameter_3ForDecember = fact_parameter_3[11]
                };
                appDB.Parameters_3.Update(TempParameters_3);
            }

            record.Note = note;
            record.Sector = sector;
            appDB.FactValue.Update(tempFactByMount);
            appDB.Jornals.Update(record);
            appDB.SaveChanges();
        }

        public void EditPlanValue(Jornal record, float[] array, float[] plan_parameter_1, float[] plan_parameter_2, float[] plan_parameter_3, string note, string sector)
        {
            PlanByMount tempPlanByMount = new PlanByMount
            {

                PlanID = record.NumberRecordingID,
                JornalID = record.NumberRecordingID,
                PlanForJanuary = array[0],
                PlanForFebruary = array[1],
                PlanForMarch = array[2],
                PlanForApril = array[3],
                PlanForMay = array[4],
                PlanForJune = array[5],
                PlanForJuly = array[6],
                PlanForAugust = array[7],
                PlanForSeptember = array[8],
                PlanForOctober = array[9],
                PlanForNovember = array[10],
                PlanForDecember = array[11]
            };
            if (plan_parameter_1 != null)
            {
                Parameters_1 TempParameters_1 = new Parameters_1
                {
                    Parameter_1ID = record.NumberRecordingID,
                    JornalID = record.NumberRecordingID,
                    PlanParameter_1ForJanuary = plan_parameter_1[0],
                    PlanParameter_1ForFebruary = plan_parameter_1[1],
                    PlanParameter_1ForMarch = plan_parameter_1[2],
                    PlanParameter_1ForApril = plan_parameter_1[3],
                    PlanParameter_1ForMay = plan_parameter_1[4],
                    PlanParameter_1ForJune = plan_parameter_1[5],
                    PlanParameter_1ForJuly = plan_parameter_1[6],
                    PlanParameter_1ForAugust = plan_parameter_1[7],
                    PlanParameter_1ForSeptember = plan_parameter_1[8],
                    PlanParameter_1ForOctober = plan_parameter_1[9],
                    PlanParameter_1ForNovember = plan_parameter_1[10],
                    PlanParameter_1ForDecember = plan_parameter_1[11]
                };
                appDB.Parameters_1.Update(TempParameters_1);
            }

            if (plan_parameter_2 != null)
            {
                Parameters_2 TempParameters_2 = new Parameters_2
                {
                    Parameter_2ID = record.NumberRecordingID,
                    JornalID = record.NumberRecordingID,
                    PlanParameter_2ForJanuary = plan_parameter_2[0],
                    PlanParameter_2ForFebruary = plan_parameter_2[1],
                    PlanParameter_2ForMarch = plan_parameter_2[2],
                    PlanParameter_2ForApril = plan_parameter_2[3],
                    PlanParameter_2ForMay = plan_parameter_2[4],
                    PlanParameter_2ForJune = plan_parameter_2[5],
                    PlanParameter_2ForJuly = plan_parameter_2[6],
                    PlanParameter_2ForAugust = plan_parameter_2[7],
                    PlanParameter_2ForSeptember = plan_parameter_2[8],
                    PlanParameter_2ForOctober = plan_parameter_2[9],
                    PlanParameter_2ForNovember = plan_parameter_2[10],
                    PlanParameter_2ForDecember = plan_parameter_2[11]
                };
                appDB.Parameters_2.Update(TempParameters_2);
            }

            if (plan_parameter_3 != null)
            {
                Parameters_3 TempParameters_3 = new Parameters_3
                {
                    Parameter_3ID = record.NumberRecordingID,
                    JornalID = record.NumberRecordingID,
                    PlanParameter_3ForJanuary = plan_parameter_3[0],
                    PlanParameter_3ForFebruary = plan_parameter_3[1],
                    PlanParameter_3ForMarch = plan_parameter_3[2],
                    PlanParameter_3ForApril = plan_parameter_3[3],
                    PlanParameter_3ForMay = plan_parameter_3[4],
                    PlanParameter_3ForJune = plan_parameter_3[5],
                    PlanParameter_3ForJuly = plan_parameter_3[6],
                    PlanParameter_3ForAugust = plan_parameter_3[7],
                    PlanParameter_3ForSeptember = plan_parameter_3[8],
                    PlanParameter_3ForOctober = plan_parameter_3[9],
                    PlanParameter_3ForNovember = plan_parameter_3[10],
                    PlanParameter_3ForDecember = plan_parameter_3[11]
                };
                appDB.Parameters_3.Update(TempParameters_3);
            }
            record.Note = note;
            record.Sector = sector;
            appDB.PlanValue.Update(tempPlanByMount);
            appDB.Jornals.Update(record);
            appDB.SaveChanges();
        }

        public void AddRecordsToJornal(int departmentID, int yearID, int workID, bool IsPlan)
        {
            
            Jornal recordJornal = new Jornal { DepartmentID = departmentID, NameOfWorksID = workID, YearID = yearID };
            if (IsPlan) recordJornal.CreatedInPlan = true;
            appDB.Jornals.Add(recordJornal);
            appDB.SaveChanges();
            appDB.FactValue.Add(new FactByMount { FactID = recordJornal.NumberRecordingID, Jornal=recordJornal });
            appDB.PlanValue.Add(new PlanByMount { PlanID = recordJornal.NumberRecordingID, Jornal = recordJornal });
            appDB.SaveChanges();
            if(appDB.Works.FirstOrDefault(x=>x.WorkID==workID).TypeRecords== "с участками и параметрами")
            {
                if (appDB.Works.FirstOrDefault(x => x.WorkID == workID).Parameter_1 != null) appDB.Parameters_1.Add(new Parameters_1 { Parameter_1ID = recordJornal.NumberRecordingID, Jornal = recordJornal });
                if (appDB.Works.FirstOrDefault(x => x.WorkID == workID).Parameter_2 != null) appDB.Parameters_2.Add(new Parameters_2 { Parameter_2ID = recordJornal.NumberRecordingID, Jornal = recordJornal });
                if (appDB.Works.FirstOrDefault(x => x.WorkID == workID).Parameter_3 != null) appDB.Parameters_3.Add(new Parameters_3 { Parameter_3ID = recordJornal.NumberRecordingID, Jornal = recordJornal });
                appDB.SaveChanges();
            }
        }

        public void AddRecordsOtherWork(OtherWork otherWork)
        {
           
            OtherWork recordOtherWork = otherWork;
            recordOtherWork.OtherPlanValue = new string[12] { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };
            recordOtherWork.OtherFactValue = new string[12] {"0","0","0","0","0","0","0","0","0","0","0","0"};
            
            appDB.OtherWorks.Add(recordOtherWork);
            appDB.SaveChanges();
        }

        public void EditOtherFactValue(OtherWork otherWork, float[] array, string note)
        {
            OtherWork tempOtherWork = otherWork;
            tempOtherWork.NoteOtherWork = note;
            string[] tempArray = new string[12];
            for (int i = 0; i < 12; i++)
            {
                tempArray[i] = array[i].ToString();
            }
            tempOtherWork.OtherFactValue = tempArray;
            appDB.Update(tempOtherWork);
            appDB.SaveChanges();

        }

        public void DeleteRecordsFromJornal(int IDRecords)
        {
            appDB.Jornals.Remove(GetOnlyRecordByID(IDRecords));
            appDB.SaveChanges();
        }

        public Jornal GetOnlyRecordByID(int IDRecord)
        {
            return appDB.Jornals.FirstOrDefault(x => x.NumberRecordingID == IDRecord);
        }

        public OtherWork GetOnlyRecordOtherWorkByID(int OtherWorkID)
        {
            return appDB.OtherWorks.FirstOrDefault(x => x.OtherWorkID==OtherWorkID);
        }

        public void AddActToJornalRecord(int recordID, int otherWorkID, ActOfWork actOfWork)
        {  
            if(recordID!=0)GetOnlyRecordByID(recordID).ActOfWorks.Add(actOfWork);
            if(otherWorkID!=0) GetOnlyRecordOtherWorkByID(otherWorkID).ActOfWorks.Add(actOfWork);   
            appDB.SaveChanges(); 
        }

        public void DeleteRecordOtherWork(int OtherWorkID)
        {
            appDB.OtherWorks.Remove(GetOnlyRecordOtherWorkByID(OtherWorkID));
            appDB.SaveChanges();
        }
    }
}
