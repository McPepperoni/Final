using AutoMapper;
using WebApi.DTOs;
using WebApi.Entities;

namespace WebApi.Helpers.MappingProfiles;

public class UserRoleProfile : Profile
{
    public UserRoleProfile()
    {
        CreateMap<UserRoleEntity, UserRoleDTO>()
        .IncludeMembers(x => x.Role)
        .ReverseMap();
    }
}