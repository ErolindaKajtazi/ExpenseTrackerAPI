using AutoMapper;
using ExpenseTrackerAPI.DTOs;
using ExpenseTrackerAPI.Models;

namespace ExpenseTrackerAPI.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Expense, ExpenseDto>();
        CreateMap<ExpenseDto, Expense>();


        // Map Category to CategoryDto
        CreateMap<Category, CategoryDto>();
    }
}