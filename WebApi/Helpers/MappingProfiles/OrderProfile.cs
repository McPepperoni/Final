using AutoMapper;
using Persistence.Entities;
using WebApi.DTOs;

namespace WebApi.Helpers.MappingProfiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<OrderEntity, OrderDTO>()
        .IncludeMembers(x => x.OrderProducts);
    }
}