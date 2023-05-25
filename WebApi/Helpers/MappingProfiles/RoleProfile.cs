using AutoMapper;
using WebApi.DTOs;
using WebApi.Entities;

namespace WebApi.Helpers.MappingProfiles;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<RoleEntity, RoleDTO>()
        .ReverseMap();

        CreateMap<RoleEntity, UserRoleDTO>();
    }
}