using AutoMapper;
using MVC.Areas.Admin.Pages.Products;
using MVC.DTOs;

namespace MVC.Helpers.MapperProfiles;

public class CategoryEntityProfile : Profile
{
    public CategoryEntityProfile()
    {
        CreateMap<CategoryDTO, AddModel.InputModel.Category>();
        CreateMap<CategoryDTO, DisplayListDTO>();
    }
}