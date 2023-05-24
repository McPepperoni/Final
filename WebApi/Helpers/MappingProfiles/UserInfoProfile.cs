using AutoMapper;
using WebApi.DTOs;
using WebApi.Entities;

namespace WebApi.Helpers.MappingProfiles;

public class UserInfoProfile : Profile
{
    public UserInfoProfile()
    {
        CreateMap<UserInfoEntity, UserDTO>();
        CreateMap<UserInfoEntity, UserInfoDTO>();
        CreateMap<UserSignUpDTO, UserInfoEntity>()
        .ReverseMap();
        CreateMap<UserPatchDTO, UserInfoEntity>();
    }
}