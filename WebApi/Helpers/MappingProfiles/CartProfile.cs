using AutoMapper;
using Persistence.Entities;
using WebApi.DTOs.CartDTO;

namespace WebApi.Helpers.MappingProfiles;

public class CartProfile : Profile
{
    public CartProfile()
    {
        CreateMap<CartEntity, CartDetailDTO>()
        .ForMember(x => x.TotalPrice, cfg => cfg.MapFrom(src => src.CartProducts.Select(x => x.Quantity * x.Product.Price).Sum()));
    }
}