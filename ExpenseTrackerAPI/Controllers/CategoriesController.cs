using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerAPI.Controllers;

[Route("api/categories/")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ExpenseDbContext _context;

    public CategoriesController(ExpenseDbContext context)
    {
        _context = context;
    }

    // GET: api/Categories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        return await _context.Categories.ToListAsync();
    }

    // POST: api/Categories
    [HttpPost]
    public async Task<ActionResult<Category>> PostCategory(Category category)
    {
        // Basic validation
        if (string.IsNullOrWhiteSpace(category.Name))
        {
            return BadRequest("Category name is required.");
        }

        
        var existingCategory = await _context.Categories
            .FirstOrDefaultAsync(c => c.Name == category.Name);

        if (existingCategory != null)
        {
            return BadRequest("Category with this name already exists.");
        }

        // Add
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCategories), new { id = category.Id }, category);
    }
}