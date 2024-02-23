using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_current_repairs.Data.Models
{
    public class NameOfWorks
    {
        /// <summary>
        /// Наименование работ по текущему ремонту
        /// </summary>
        [Key]
        public int WorkID { get; set; }
        [Required(ErrorMessage = "Не указано наименование работ")]
        public string NameOfWork { get; set; }
        [Required(ErrorMessage = "Не указано описание")]
        public string Discription { get; set; }
        [Required(ErrorMessage = "Не указана переодичность")]
        public string Periodicity { get; set; }
        [Required(ErrorMessage = "Не указана единица измерения")]
        public string Unit { get; set; }
        public bool IntegerValue { get; set; }
        public bool Active { get; set;}
        public string TypeRecords{ get; set; }
        public string Parameter_1 { get; set; }
        public string Parameter_2 { get; set; }
        public string Parameter_3 { get; set; }
        public List<Jornal> Records { get; set; } = new List<Jornal>();
        public int GroupNameOfWorksID { get; set; }
        public GroupNameOfWorks GroupName { get; set; }

    }
}
