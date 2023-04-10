using System.ComponentModel.DataAnnotations;

namespace Expense.Models.ViewModel
{
    public class RegisterViewModel
    {

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password Doesn't match")]
        public string Confirmpassword { get; set; }
    }
}
