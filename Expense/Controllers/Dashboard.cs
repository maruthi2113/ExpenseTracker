using Expense.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Expense.Controllers
{
    public class Dashboard : Controller
    {
        private readonly ApplicationDbContext _context;
        public Dashboard(ApplicationDbContext context)
        { 
        _context=context;
        }
        public async Task<ActionResult> Index()
        {
            // last 7 days
            DateTime startDate = DateTime.Today.AddDays(-6);
            DateTime endDate = DateTime.Today;


            List<Transaction> SelectedTransaction = await _context.Transcations.Include(x => x.Category)
                .Where(x => x.Date >= startDate && x.Date <= endDate).ToListAsync();

            // Total income
            int TotalIncome = SelectedTransaction.Where(x => x.Category.Type == "Income")
                .Sum(x => x.Amount);
            ViewBag.TotalIncome = TotalIncome.ToString();

            // Total Expense
            int TotalExpense = SelectedTransaction.Where(x => x.Category.Type == "Expense")
                .Sum(x => x.Amount);
            ViewBag.TotalExpense = TotalExpense.ToString();

            // Expense By Category
            ViewBag.DoughnutChartData = SelectedTransaction.Where(i => i.Category.Type == "Expense")
                .GroupBy(j => j.Category.CategoryId)
                .Select(k => new {
                    categoryTitleWithIcon = k.First().Category.Icon + " " + k.First().Category.Title,
                    amount = k.Sum(j => j.Amount),
                    formattedAmount = k.Sum(j => j.Amount).ToString(),
                }).OrderByDescending(l => l.amount).ToList();

            //Balance
            int Balance = TotalIncome - TotalExpense;
            ViewBag.Balance = Balance.ToString();


            //spline cart income vs expense
            List<SplineChartData> IncomeSummary = SelectedTransaction
            .Where(i => i.Category.Type == "Income")
            .GroupBy(j => j.Date)
            .Select(k => new SplineChartData()
            {
                day = k.First().Date.ToString("dd-MMM"),
                income = k.Sum(l => l.Amount)
            }).ToList();
            //Expense
            List<SplineChartData> ExpenseSummary = SelectedTransaction
            .Where(i => i.Category.Type == "Expense")
            .GroupBy(j => j.Date)
            .Select(k => new SplineChartData()
            {
                day = k.First().Date.ToString("dd-MMM"),
                expense = k.Sum(l => l.Amount)
            }).ToList();
            //Combine
            String[] Last7Days = Enumerable.Range(0, 7)
                .Select(i=>startDate.AddDays(i).ToString("dd-MMM"))
                .ToArray();
            ViewBag.SplineChartData = from day in Last7Days
                                      join income in IncomeSummary on day equals income.day into dayIncomJoined
                                      from income in dayIncomJoined.DefaultIfEmpty()
                                      join expense in ExpenseSummary on day equals expense.day into expenseJoined
                                      from expense in expenseJoined.DefaultIfEmpty()
                                      select new
                                      {
                                          day = day,
                                          income = income == null ? 0 : income.income,
                                          expense = expense == null ? 0 : expense.expense,
                                      };

            // recent transactions
            ViewBag.RecentTransactions = await _context.Transcations
                .Include(i => i.Category) 
                .OrderByDescending(j => j.Date)
                .Take(6)
                .ToListAsync();

            return View();
        }
    }
}

public class SplineChartData
{
    public String day;
    public int income;
    public int expense;
}