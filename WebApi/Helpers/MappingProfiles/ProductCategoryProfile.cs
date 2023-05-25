using AutoMapper;
using WebApi.DTOs;
using WebApi.Entities;

namespace WebApi.Helpers.MappingProfiles;

public class ProductCategoryProfile : Profile
{
    public ProductCategoryProfile()
    {
        CreateMap<ProductCategoryEntity, ProductCategoryDTO>()
        .IncludeMembers(x => x.Category)
        .IncludeMembers(x => x.Product)
        .ReverseMap();

        CreateMap<List<ProductCategoryEntity>, CategoryDTO>();
        CreateMap<List<ProductCategoryEntity>, ProductDTO>();
    }
}