using AutoMapper;
using Persistence.Entities;
using WebApi.DTOs.CategoryDTO;
using WebApi.DTOs.ProductCategoryDTO;
using WebApi.DTOs.ProductDTO;

namespace WebApi.Helpers.MappingProfiles;

public class ProductCategoryProfile : Profile
{
    public ProductCategoryProfile()
    {
        CreateMap<ProductCategoryEntity, ProductCategoryDetailDTO>()
        .IncludeMembers(x => x.Category)
        .IncludeMembers(x => x.Product)
        .ReverseMap();

        CreateMap<List<ProductCategoryEntity>, CategoryDetailDTO>();
        CreateMap<List<ProductCategoryEntity>, ProductDetailDTO>();
    }
}