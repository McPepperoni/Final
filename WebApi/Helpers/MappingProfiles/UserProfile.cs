using AutoMapper;
using WebApi.DTOs;
using WebApi.Entities;

namespace WebApi.Helpers.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserEntity, UserDTO>()
        .IncludeMembers(x => x.UserInfo);

        CreateMap<UserEntity, UserSignUpDTO>()
        .IncludeMembers(x => x.UserInfo)
        .ReverseMap();
        CreateMap<UserPatchDTO, UserEntity>();
    }
}