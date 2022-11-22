using Microsoft.EntityFrameworkCore;

namespace Expense.Models
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Transaction> Transcations { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
