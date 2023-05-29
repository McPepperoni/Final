using AutoMapper;
using Persistence.Entities;
using WebApi.DTOs;

namespace WebApi.Helpers.MappingProfiles;

public class CartProfile : Profile
{
    public CartProfile()
    {
        CreateMap<CartEntity, CartDTO>();
    }
}