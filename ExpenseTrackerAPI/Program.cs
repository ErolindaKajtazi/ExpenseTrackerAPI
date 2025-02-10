using Microsoft.OpenApi.Models;
using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.ExpenseStatistics;
using ExpenseTrackerAPI.Profiles;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure SQL Server Connection
builder.Services.AddDbContext<ExpenseDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Swagger Service
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Expense Tracker API",
        Version = "v1",
        Description = "An API for tracking personal expenses and budgets.",
    });
});

builder.Services.AddAutoMapper(typeof(MappingProfile)); 
builder.Services.AddScoped<ExpenseStatisticsService>();
var app = builder.Build();

// Enable Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Expense Tracker API v1");
        c.RoutePrefix = string.Empty;
    });
}

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();