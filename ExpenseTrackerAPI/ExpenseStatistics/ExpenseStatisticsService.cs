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

    // Most expensive
    public async Task<Expense> GetMostExpensiveExpenseAsync()
    {
        return await _context.Expenses
            .OrderByDescending(e => e.Amount)
            .FirstOrDefaultAsync();
    }

    // Least
    public async Task<Expense> GetLeastExpensiveExpenseAsync()
    {
        return await _context.Expenses
            .OrderBy(e => e.Amount)
            .FirstOrDefaultAsync();
    }

    public async Task<decimal> GetAverageDailyExpensesAsync()
    {
        var firstExpense = await _context.Expenses.OrderBy(e => e.Date).FirstOrDefaultAsync();
    
        if (firstExpense == null) return 0; // No expenses exist

        var totalDays = Math.Max(1, (DateTime.Now - firstExpense.Date).Days);
        var totalExpenses = await _context.Expenses.SumAsync(e => e.Amount);

        return totalExpenses / totalDays;
    }

    public async Task<decimal> GetAverageMonthlyExpensesAsync()
    {
        var firstExpense = await _context.Expenses.OrderBy(e => e.Date).FirstOrDefaultAsync();
    
        if (firstExpense == null) return 0;

        var totalMonths = Math.Max(1, (DateTime.Now.Year - firstExpense.Date.Year) * 12 + (DateTime.Now.Month - firstExpense.Date.Month));
        var totalExpenses = await _context.Expenses.SumAsync(e => e.Amount);

        return totalExpenses / totalMonths;
    }

    public async Task<decimal> GetAverageYearlyExpensesAsync()
    {
        var firstExpense = await _context.Expenses.OrderBy(e => e.Date).FirstOrDefaultAsync();
    
        if (firstExpense == null) return 0;

        var totalYears = Math.Max(1, DateTime.Now.Year - firstExpense.Date.Year);
        var totalExpenses = await _context.Expenses.SumAsync(e => e.Amount);

        return totalExpenses / totalYears;
    }

    
    public async Task<decimal> GetTotalExpensesAsync()
    {
        return await _context.Expenses.SumAsync(e => e.Amount);
    }
}