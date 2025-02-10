using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerAPI.Controllers;

[Route("api/settings")]
[ApiController]
public class SettingsController : ControllerBase
{
    private readonly ExpenseDbContext _context;

    public SettingsController(ExpenseDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<Settings>> PostSettings(Settings settings)
    {
        _context.Settings.Add(settings);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(PostSettings), new { id = settings.Id }, settings);
    }
}