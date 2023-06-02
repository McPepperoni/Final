using AutoMapper;
using Persistence.Entities;
using WebApi.DTOs;

namespace WebApi.Helpers.MappingProfiles;

public class CartProductProfile : Profile
{
    public CartProductProfile()
    {
        CreateMap<CartProductEntity, CartProductDTO>();
    }
}