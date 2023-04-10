using System.ComponentModel.DataAnnotations;

namespace Expense.Models.ViewModel
{
    public class ChangeViewModel
    {

       
     
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password Doesn't match")]
        public string Confirmpassword { get; set; }
    }
}
