using Microsoft.AspNetCore.Identity;

namespace Expense.Models
{
    public class AppUser:IdentityUser
    {
        public string? Street { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        public string? ProfileImg { get; set; }

        public string? Number { get; set;}
    }
}
