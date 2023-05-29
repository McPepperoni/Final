using AutoMapper;
using Persistence.Entities;
using WebApi.DTOs;

namespace WebApi.Helpers.MappingProfiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CategoryEntity, CategoryDTO>()
        .ReverseMap();
    }
}