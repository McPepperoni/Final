using AutoMapper;
using Persistence.Entities;
using WebApi.DTOs;

namespace WebApi.Helpers.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<AuthRegisterDTO, UserEntity>();
    }
}