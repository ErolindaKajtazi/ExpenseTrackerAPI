using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTrackerAPI.Models;

public class Settings
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal OverallBudget { get; set; }
}