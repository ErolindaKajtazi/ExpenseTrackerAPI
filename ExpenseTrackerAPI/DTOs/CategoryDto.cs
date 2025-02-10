namespace ExpenseTrackerAPI.DTOs;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Budget { get; set; }
}