using AutoMapper;
using Persistence.Entities;
using WebApi.DTOs.CategoryDTO;

namespace WebApi.Helpers.MappingProfiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CategoryEntity, CategoryDetailDTO>()
        .ReverseMap();
    }
}