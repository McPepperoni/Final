using AutoMapper;
using Persistence.Entities;
using WebApi.DTOs;

namespace WebApi.Helpers.MappingProfiles;

public class OrderProductProfile : Profile
{
    public OrderProductProfile()
    {
        CreateMap<OrderProductEntity, OrderProductDTO>()
        .IncludeMembers(x => x.Product);

        CreateMap<List<OrderProductEntity>, OrderDTO>();
    }
}