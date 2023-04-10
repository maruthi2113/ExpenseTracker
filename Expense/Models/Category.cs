using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expense.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        //public string AppUserId { get;set; }
        //public AppUser? AppUser { get;set; }
        
        [Column(TypeName="nvarchar(50)")]
        [Required(ErrorMessage ="Title is Required.")]
        public string Title { get; set; }

        [Column(TypeName = "nvarchar(5)")]
        public String Icon { get; set; } = "";
        [Column(TypeName = "nvarchar(10)")]
        public String Type { get; set; } = "Expense";
        [NotMapped]
        public string? TitleWithIcon {
            get
            { 
                return this.Icon+" "+ this.Title;
            }
                
               }

    }
}
