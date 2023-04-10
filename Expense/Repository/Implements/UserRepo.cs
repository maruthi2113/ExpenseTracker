using Expense.Models;
using Expense.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Expense.Repository.Implements
{
    public class UserRepo:IUserRepo
    {
        private readonly ApplicationDbContext _context;
        public UserRepo(ApplicationDbContext context) {
          _context = context;
        }

        public void Add(AppUser user)
        {
            _context.Users.Add(user);   
            _context.SaveChanges();
        }

        public async Task<AppUser> Get(string userid)
        {
            return await _context.Users.FirstOrDefaultAsync(n=>n.Id==userid);
        }

        public void Update(AppUser user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}
