using AutoMapper;
using Persistence.Entities;
using WebApi.DTOs;

namespace WebApi.Helpers.MappingProfiles;

public class JWTProfile : Profile
{
    public JWTProfile()
    {
        CreateMap<JWTTokenEntity, JWTTokenDTO>();
    }
}