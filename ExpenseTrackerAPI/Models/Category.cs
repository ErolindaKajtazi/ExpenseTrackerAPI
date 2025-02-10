using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTrackerAPI.Models;

public class Category
{
    [Key] public int Id { get; set; }

    [Required] public string Name { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Budget { get; set; }

    // One-to-Many Relationship: A category has many expenses
    public List<Expense> Expenses { get; set; } = new();
}