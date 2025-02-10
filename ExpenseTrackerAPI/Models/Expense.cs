using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ExpenseTrackerAPI.Models;

public class Expense
{
    [Key] public int Id { get; set; }

    [Required] public string Description { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Required] public DateTime Date { get; set; }

    // Foreign Key
    [Required] public int CategoryId { get; set; }

    [JsonIgnore]
    [ForeignKey("CategoryId")] public Category? Category { get; set; } = null!;
}