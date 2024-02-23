using System.ComponentModel.DataAnnotations;

namespace Plan_current_repairs.Data.Models
{
    public class ChangePassword
    {
        public int EmployeeID { get; set; }
        public string FullName { get; set; }

        [MinLength(5)]
        public string Password { get; set; }
        [MinLength(5)]
        public string ConfirmPassword { get; set; }
    }
}
