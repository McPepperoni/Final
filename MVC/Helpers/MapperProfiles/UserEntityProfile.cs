using AutoMapper;
using MVC.Areas.Identity.Pages.Account;
using Persistence.Entities;

namespace MVC.Helpers.MapperProfiles;

public class UserEntityProfile : Profile
{
    public UserEntityProfile()
    {
        CreateMap<RegisterModel.InputModel, UserEntity>()
        .ForMember(x => x.UserName, o => o.MapFrom(src => src.Email));
    }
}