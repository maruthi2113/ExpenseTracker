using System.ComponentModel.DataAnnotations;

namespace Expense.Models.ViewModel
{
    public class EditViewModel
    {

        [Required]
        public string Name { get; set; }

        public string? Street { get;set; }
        public string? City { get; set; }   
       
        public string? Country { get; set; }
        public string? Number { get;set; }
        public IFormFile? Image { get;set; }
        public string? ProfileImg { get; set; }

    }
}
