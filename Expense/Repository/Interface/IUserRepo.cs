using Expense.Models;

namespace Expense.Repository.Interface
{
    public interface IUserRepo
    {



        public void Add(AppUser user);
        public void Update(AppUser user);   
        public Task<AppUser> Get(string userid);

    }
}
