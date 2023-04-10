using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Expense.Models;
using Expense.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace Expense.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        public TransactionController(ApplicationDbContext context,IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            _context = context;
        }

        // GET: Transaction
        public async Task<IActionResult> Index()
        {
            var currentuderid = _contextAccessor.HttpContext.User.GetUserId();
            var applicationDbContext = _context.Transcations.Include(t => t.Category).Where(x=>x.AppUserId==currentuderid);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Transaction/Create
        [Authorize]
        public IActionResult AddOrEdit(int id=0)
        {
            var currentuser = _contextAccessor.HttpContext.User.GetUserId();
            PopulateCategories();
            if (id == 0)
            {
                var newt = new Transaction();
                newt.AppUserId = currentuser;
                return View(newt);
            }
            else
            return View(_context.Transcations.Find(id));
           
        }

        // POST: Transaction/C
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddOrEdit([Bind("TramscationId,CategoryId,AppUserId,Amount,Date,Note")] Transaction transaction)
        {
            var currentuser = _contextAccessor.HttpContext.User.GetUserId();
            transaction.AppUserId = currentuser;
            if (ModelState.IsValid)
            {
               
                if (transaction.TramscationId == 0)
                {
                    //transaction.AppUserId = currentuser;
                    _context.Add(transaction);
                   
                }
                else
                    _context.Update(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateCategories();
            return View(transaction);
        }

        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Transcations == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Transcations'  is null.");
            }
            var transaction = await _context.Transcations.FindAsync(id);
            if (transaction != null)
            {
                _context.Transcations.Remove(transaction);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [NonAction]
        public void PopulateCategories()
        {
            var CategoryCollection = _context.Categories.ToList();
            Category DefaultCategory = new Category()
            {
                CategoryId=0,Title="Choose a Category"
            };
            CategoryCollection.Insert(0,DefaultCategory);
            ViewBag.Categories = CategoryCollection;
        }
       
    }
}
