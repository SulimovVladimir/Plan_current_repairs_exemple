using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan_current_repairs.Data.Models
{
    public class Year
    {
        [Key]
        public int YearID { get; set; }
        [Required(ErrorMessage = "Не указано значение года")]
        [RegularExpression(@"[0-9]{4}", ErrorMessage = "Введите корректный год")]
        public ushort Years { get; set; }
        public List<Jornal> records { get; set; } = new List<Jornal>();
        public List<Status> Status { get; set; } = new List<Status>();
        public List<ActOfWork> ActOfWorks { get; set; } = new List<ActOfWork>();
    }
}
