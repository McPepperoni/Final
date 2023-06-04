using AutoMapper;
using MVC.DTOs;

namespace MVC.Helpers.MapperProfiles;

public class ProductEntityProfile : Profile
{
    public ProductEntityProfile()
    {
        CreateMap<ProductDTO, DisplayListDTO>();
    }
}