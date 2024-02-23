using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_current_repairs.Data.Models
{
    /// <summary>
    /// Класс модели, содержащий информацию о цехах и участках УС
    /// </summary>
    public class Department
    {
        [Key]
        public int ID { get; set; }
        [Required (ErrorMessage ="Не указано название филиала")]
        [RegularExpression(@"[А-Яа-яЁё\s]{5,50}", ErrorMessage = "Используйте только русский язык")]
        public string NameDepartment { get; set; }
        [Required(ErrorMessage = "Не указан руководитель филиала")]
        [RegularExpression(@"[А-Яа-яЁё\s]{5,50}", ErrorMessage = "Используйте только русский язык")]
        public string DirectorDepartment { get; set; }
        public string StandartNumberAct { get; set; }
        public List<Employee> Employees { get; set; } = new List<Employee>();
        public List<Jornal> RecordsJornal { get; set; } = new List<Jornal>();
        public List<DictionarySector> DictionarySector { get; set; } = new List<DictionarySector>();
        public List<Status> Status { get; set; } = new List<Status>();
        public List<ActOfWork> ActOfWorks { get; set; } = new List<ActOfWork>();
    }
}
