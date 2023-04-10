using System.ComponentModel.DataAnnotations;

namespace Expense.Models.ViewModel
{
    public class LoginViewModel
    {

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get;set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
       
        
    }
}
