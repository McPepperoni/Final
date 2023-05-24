using AutoMapper;
using WebApi.DTOs;

namespace WebApi.Test.MappingProfiles;

public class UserControllerProfile : Profile
{
    public UserControllerProfile()
    {
        CreateMap<UserDTO, UserSignUpDTO>().IncludeMembers(x => x.UserInfo).ReverseMap();
        CreateMap<UserInfoDTO, UserSignUpDTO>().ReverseMap();
    }
}