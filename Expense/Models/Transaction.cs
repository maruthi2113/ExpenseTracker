using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expense.Models
{
    public class Transaction
    {
        [Key]
        public int TramscationId { get; set; }

        public string AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select a Category")]

        public int CategoryId { get; set; }

        public  Category? Category { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Amount should be greater than 0")]

        public int Amount { get; set; }
        
        [Column(TypeName = "nvarchar(75)")]
        public String? Note { get; set; }

        public DateTime Date { get; set; }=DateTime.Now;

        [NotMapped]
        public String? CategoryTitleWithIcon {
            get {
                return Category==null?"":Category.Icon+" "+Category.Title;
            }
        }

        [NotMapped]
        public String? FormattedAmount
        {
            get
            {
                return ((Category==null||Category.Type=="Expense")?"-":"+")+Amount.ToString(); 
            }
        }

    }
}
