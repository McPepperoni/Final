using AutoMapper;
using Persistence.Entities;
using WebApi.DTOs.OrderDTO;

namespace WebApi.Helpers.MappingProfiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<OrderEntity, OrderDetailDTO>()
        .IncludeMembers(x => x.Products);
    }
}