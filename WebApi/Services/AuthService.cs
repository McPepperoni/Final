using System.IdentityModel.Tokens.Jwt;
using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Constants;
using WebApi.Contexts;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Helpers.JWT;
using WebApi.Middleware.ExceptionHandler;
using WebApi.Services;
using WebApi.Settings;

public interface IAuthService
{
    Task<ResponseRequestTokenDTO> RequestToken(UserDTO user);
}

public class AuthService : BaseService<CartEntity>, IAuthService
{
    private readonly JWTHelper _jwtHelper;
    private readonly AppSettings _appSettings;
    public AuthService(ApplicationDbContext dbContext, JWTHelper jwtHelper, IMapper mapper, AppSettings appSettings) : base(dbContext, mapper)
    {
        _jwtHelper = jwtHelper;
        _appSettings = appSettings;
    }

    public async Task<ResponseRequestTokenDTO> RequestToken(UserDTO user)
    {
        var cart = await _dbSet.Where(x => x.UserId == user.Id).FirstOrDefaultAsync();

        if (cart == null)
        {
            throw new AppException(HttpStatusCode.NotFound, "This user has not been registered");
        }

        var token = _jwtHelper.Create(user, DateTime.UtcNow.AddMinutes(15));

        return new()
        {
            AccessToken = token.Token,
            User = user
        };
    }
}