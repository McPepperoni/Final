using AutoMapper;
using Persistence.Entities;
using WebApi.DTOs.UserDTO;

namespace WebApi.Helpers.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserEntity, UserDetailDTO>();
        CreateMap<CreateUserDTO, UserEntity>();
    }
}