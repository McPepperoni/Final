using AutoMapper;
using Persistence.Entities;
using WebApi.DTOs;

namespace WebApi.Helpers.MappingProfiles;

public class CartProfile : Profile
{
    public CartProfile()
    {
        CreateMap<CartEntity, CartDTO>()
        .ForMember(x => x.TotalPrice, cfg => cfg.MapFrom(src => src.CartProducts.Select(x => x.Quantity * x.Product.Price).Sum()));
    }
}