using ExpenseTrackerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerAPI.Data;

public class ExpenseDbContext : DbContext
{
    public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options) : base(options)
    {
    }

    public DbSet<Expense> Expenses { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Lidhja One to Many
        modelBuilder.Entity<Expense>().HasOne(e => e.Category)
            .WithMany(c => c.Expenses).HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Cascade); // if category is deleted then all its related expenses are also deleted

        // unique category names
        modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique();
    }
}