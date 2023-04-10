using System.ComponentModel.DataAnnotations;

namespace Expense.Models.ViewModel
{
    public class ForgotViewModel
    {

        
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password Doesn't match")]
        public string Confirmpassword { get; set; }
    }
}
