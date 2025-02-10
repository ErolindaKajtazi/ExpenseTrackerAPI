using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTrackerAPI.Models;

public class Expense
{
    [Key] public int Id { get; set; }

    [Required] public string Description { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Required] public DateTime Date { get; set; }

    // Foreign Key (Ensures every expense is linked to a category)
    [Required] public int CategoryId { get; set; }

    [ForeignKey("CategoryId")] public Category Category { get; set; } = null!;
}