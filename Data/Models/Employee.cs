using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_current_repairs.Data.Models
{
    /// <summary>
    /// Работники организации
    /// </summary>
    public class Employee
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "Не указано ФИО сотрудника")]
        [RegularExpression(@"[А-Яа-яЁё\s]{5,50}",ErrorMessage ="Используйте только русский язык")]
        public string FullName { get; set; }
        public string Post { get; set; }
        public string Role { get; set; }

        //[Required(ErrorMessage = "Не указана учетная запись сотрудника")]
        public string Account { get; set; }

        //[Required(ErrorMessage = "Не указан пароль")]
        [MinLength(5)]
        public string Password { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        [UIHint("Boolean")]
        public int DepartmentID { get; set; }
        public Department Department { get; set; }

    }
}
