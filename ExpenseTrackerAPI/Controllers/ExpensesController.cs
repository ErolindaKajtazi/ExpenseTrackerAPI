using AutoMapper;
using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.DTOs;
using ExpenseTrackerAPI.ExpenseStatistics;
using ExpenseTrackerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerAPI.Controllers;

[Route("api/expenses/")]
[ApiController]
public class ExpensesController : ControllerBase
{
    private readonly ExpenseDbContext _context;
    private readonly IMapper _mapper;
    private readonly ExpenseStatisticsService _statisticsService;

    public ExpensesController(ExpenseDbContext context, IMapper mapper, ExpenseStatisticsService statisticsService)
    {
        _context = context;
        _mapper = mapper;
        _statisticsService = statisticsService;
    }

    // GET: api/Expenses
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpenses()
    {
        // Fetch the expenses from the database and include the associated categories
        var expenses = await _context.Expenses.Include(e => e.Category).ToListAsync();

        // Use AutoMapper to map the list of Expense objects to a list of ExpenseDto
        var expensesDto = _mapper.Map<List<ExpenseDto>>(expenses);

        return Ok(expensesDto);
    }

    // POST: api/Expenses
    [HttpPost]
    public async Task<ActionResult<Expense>> PostExpense([FromBody] ExpenseDto expenseDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Map ExpenseDto to Expense using AutoMapper
        var expense = _mapper.Map<Expense>(expenseDto);

        // Find the category by Id
        var category = await _context.Categories.FindAsync(expense.CategoryId);
        if (category == null)
        {
            return BadRequest("Category not found.");
        }

        expense.Category = category;

        // Check if the category budget is exceeded
        var totalCategoryAmount = await _context.Expenses
            .Where(e => e.CategoryId == expense.CategoryId)
            .SumAsync(e => e.Amount);

        if (totalCategoryAmount + expense.Amount > category.Budget)
        {
            return BadRequest("Category budget exceeded.");
        }

        // Check if the overall budget is exceeded
        var overallBudget = await _context.Settings.FirstOrDefaultAsync();
        if (overallBudget == null)
        {
            return BadRequest("Overall budget not defined.");
        }

        var totalExpenses = await _context.Expenses.SumAsync(e => e.Amount);
        if (totalExpenses + expense.Amount > overallBudget.OverallBudget)
        {
            return BadRequest("Overall budget exceeded.");
        }

        // Save the expense to the database
        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetExpense), new { id = expense.Id }, expense);
    }

    // GET: api/Expenses/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ExpenseDto>> GetExpense(int id)
    {
        var expense = await _context.Expenses.Include(e => e.Category).FirstOrDefaultAsync(e => e.Id == id);

        if (expense == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<ExpenseDto>(expense));
    }

    // PUT: api/Expenses/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutExpense(int id, ExpenseDto expenseDto)
    {
        if (id != expenseDto.Id)
        {
            return BadRequest();
        }

        var expense = _mapper.Map<Expense>(expenseDto);
        _context.Entry(expense).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Expenses/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpense(int id)
    {
        var expense = await _context.Expenses.FindAsync(id);
        if (expense == null)
        {
            return NotFound();
        }

        _context.Expenses.Remove(expense);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    
    // Other methods
    // GET: api/Expenses/most-expensive
    [HttpGet("most-expensive")]
    public async Task<ActionResult<Expense>> GetMostExpensiveExpense()
    {
        var expense = await _statisticsService.GetMostExpensiveExpenseAsync();
        if (expense == null)
        {
            return NotFound("No expenses found.");
        }
        return Ok(expense);
    }

    // GET: api/Expenses/least-expensive
    [HttpGet("least-expensive")]
    public async Task<ActionResult<Expense>> GetLeastExpensiveExpense()
    {
        var expense = await _statisticsService.GetLeastExpensiveExpenseAsync();
        if (expense == null)
        {
            return NotFound("No expenses found.");
        }
        return Ok(expense);
    }

    // GET: api/Expenses/average-daily
    [HttpGet("average-daily")]
    public async Task<ActionResult<decimal>> GetAverageDailyExpenses()
    {
        var avgDaily = await _statisticsService.GetAverageDailyExpensesAsync();
        return Ok(avgDaily);
    }

    // GET: api/Expenses/average-monthly
    [HttpGet("average-monthly")]
    public async Task<ActionResult<decimal>> GetAverageMonthlyExpenses()
    {
        var avgMonthly = await _statisticsService.GetAverageMonthlyExpensesAsync();
        return Ok(avgMonthly);
    }

    // GET: api/Expenses/average-yearly
    [HttpGet("average-yearly")]
    public async Task<ActionResult<decimal>> GetAverageYearlyExpenses()
    {
        var avgYearly = await _statisticsService.GetAverageYearlyExpensesAsync();
        return Ok(avgYearly);
    }

    // GET: api/Expenses/total-expenses
    [HttpGet("total-expenses")]
    public async Task<ActionResult<decimal>> GetTotalExpenses()
    {
        var total = await _statisticsService.GetTotalExpensesAsync();
        return Ok(total);
    }
}