using AutoMapper;
using Persistence.Entities;
using WebApi.DTOs;

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