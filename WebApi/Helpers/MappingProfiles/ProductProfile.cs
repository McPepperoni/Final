using AutoMapper;
using Persistence.Entities;
using WebApi.DTOs.OrderProductDTO;
using WebApi.DTOs.ProductCategoryDTO;
using WebApi.DTOs.ProductDTO;

namespace WebApi.Helpers.MappingProfiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<ProductEntity, ProductDetailDTO>()
        .IncludeMembers(x => x.Categories);

        CreateMap<ProductEntity, ProductCategoryDetailDTO>();
        CreateMap<ProductCreateDTO, ProductEntity>();
        CreateMap<ProductEntity, OrderProductDetailDTO>();
    }
}