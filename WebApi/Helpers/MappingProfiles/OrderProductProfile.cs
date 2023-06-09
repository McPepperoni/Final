using AutoMapper;
using Persistence.Entities;
using WebApi.DTOs.OrderDTO;
using WebApi.DTOs.OrderProductDTO;

namespace WebApi.Helpers.MappingProfiles;

public class OrderProductProfile : Profile
{
    public OrderProductProfile()
    {
        CreateMap<OrderProductEntity, OrderProductDetailDTO>()
        .IncludeMembers(x => x.Product);

        CreateMap<List<OrderProductEntity>, OrderDetailDTO>();
    }
}