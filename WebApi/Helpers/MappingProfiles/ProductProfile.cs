using AutoMapper;
using WebApi.DTOs;
using WebApi.Entities;

namespace WebApi.Helpers.MappingProfiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<ProductEntity, ProductDTO>()
        .IncludeMembers(x => x.Categories);

        CreateMap<ProductEntity, ProductCategoryDTO>();
    }
}