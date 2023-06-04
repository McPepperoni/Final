using AutoMapper;
using MVC.Areas.Identity.Pages.Account;
using MVC.DTOs;
using Persistence.Entities;

namespace MVC.Helpers.MapperProfiles;

public class UserEntityProfile : Profile
{
    public UserEntityProfile()
    {
        CreateMap<CreateUserDTO, UserEntity>()
        .ForMember(x => x.UserName, o => o.MapFrom(src => src.Email));

        CreateMap<UserDTO, DisplayListDTO>()
        .ForMember(x => x.Name, o => o.MapFrom(src => src.FullName));
        CreateMap<CategoryDTO, DisplayListDTO>();
    }
}