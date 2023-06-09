using AutoMapper;
using Persistence.Entities;
using WebApi.DTOs.CartDTO;

namespace WebApi.Helpers.MappingProfiles;

public class CartProductProfile : Profile
{
    public CartProductProfile()
    {
        CreateMap<CartProductEntity, CartProductDetailDTO>();
    }
}