using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerAPI.ExpenseStatistics;

public class ExpenseStatisticsService
{
    private readonly ExpenseDbContext _context;

    public ExpenseStatisticsService(ExpenseDbContext context)
    {
        _context = context;
    }

    // Most expensive expense
    public async Task<Expense> GetMostExpensiveExpenseAsync()
    {
        return await _context.Expenses
            .OrderByDescending(e => e.Amount)
            .FirstOrDefaultAsync();
    }

    // Least expensive expense
    public async Task<Expense> GetLeastExpensiveExpenseAsync()
    {
        return await _context.Expenses
            .OrderBy(e => e.Amount)
            .FirstOrDefaultAsync();
    }

    // Average daily expenses
    public async Task<decimal> GetAverageDailyExpensesAsync()
    {
        var totalDays = (DateTime.Now - _context.Expenses.Min(e => e.Date)).Days;
        var totalExpenses = await _context.Expenses.SumAsync(e => e.Amount);
        return totalDays > 0 ? totalExpenses / totalDays : 0;
    }

    // Average monthly expenses
    public async Task<decimal> GetAverageMonthlyExpensesAsync()
    {
        var totalMonths = (DateTime.Now.Year - _context.Expenses.Min(e => e.Date).Year) * 12 + DateTime.Now.Month - _context.Expenses.Min(e => e.Date).Month;
        var totalExpenses = await _context.Expenses.SumAsync(e => e.Amount);
        return totalMonths > 0 ? totalExpenses / totalMonths : 0;
    }

    // Average yearly expenses
    public async Task<decimal> GetAverageYearlyExpensesAsync()
    {
        var totalYears = DateTime.Now.Year - _context.Expenses.Min(e => e.Date).Year;
        var totalExpenses = await _context.Expenses.SumAsync(e => e.Amount);
        return totalYears > 0 ? totalExpenses / totalYears : 0;
    }

    // Total expenses
    public async Task<decimal> GetTotalExpensesAsync()
    {
        return await _context.Expenses.SumAsync(e => e.Amount);
    }
}