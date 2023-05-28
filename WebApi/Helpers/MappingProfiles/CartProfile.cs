using AutoMapper;
using WebApi.DTOs;
using WebApi.Entities;

namespace WebApi.Helpers.MappingProfiles;

public class CartProfile : Profile
{
    public CartProfile()
    {
        CreateMap<CartEntity, CartDTO>();
    }
}