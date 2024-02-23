using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan_current_repairs.Data.Models
{
    public class GroupNameOfWorks
    {
        [Key]
       
        public int GroupID { get; set; }
        [Required(ErrorMessage = "Не указано название группы")]
        public string NameGroup { get; set; }
        public List<NameOfWorks> Works { get; set; } = new List<NameOfWorks>();
      
    }
}
