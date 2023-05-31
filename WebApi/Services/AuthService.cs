using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Entities;
using Persistence.Managers;
using WebApi.Constants;
using WebApi.DTOs;
using WebApi.Helpers.JWT;
using WebApi.Middleware.ExceptionHandler;
using WebApi.Services;
using WebApi.Settings;
public interface IAuthService
{
    Task<JWTTokenDTO> Login(AuthLoginDTO authLogin);
    Task Logout(string token);
    Task<JWTTokenDTO> RefreshToken(string token);
}

public class AuthService : BaseService<UserEntity>, IAuthService
{
    private readonly JWTHelper _jwtHelper;
    private readonly DbSet<JWTTokenEntity> _whiteListedToken;
    private readonly AppSettings _appSettings;
    private readonly FinalUserManager _userManager;
    private readonly FinalSignInManager _signInManager;
    public AuthService(ApplicationDbContext dbContext, JWTHelper jwtHelper, IMapper mapper, AppSettings appSettings, FinalUserManager userManager, FinalSignInManager signInManager) : base(dbContext, mapper)
    {
        _jwtHelper = jwtHelper;
        _appSettings = appSettings;
        _userManager = userManager;
        _signInManager = signInManager;
        _whiteListedToken = _dbContext.WhiteListedToken;
    }

    public async Task<JWTTokenDTO> Login(AuthLoginDTO authLogin)
    {
        var result = await _signInManager.SignInWithEmailPasswordAsync(authLogin.Email, authLogin.Password, true);

        if (!result.Succeeded)
        {
            throw new AppException(HttpStatusCode.BadRequest, String.Format(ErrorMessages.BAD_REQUEST_FAILED, "Login"));
        }

        var token = await _jwtHelper.Create(await _userManager.FindByEmailAsync(authLogin.Email), DateTime.UtcNow.AddHours(1));
        await _whiteListedToken.AddAsync(token);

        await _dbContext.SaveChangesAsync();


        return _mapper.Map<JWTTokenDTO>(token);
    }

    public async Task<JWTTokenDTO> RefreshToken(string ExpiredToken)
    {
        var token = await _whiteListedToken.Where(x => x.Token == ExpiredToken).FirstOrDefaultAsync();
        if (token == null)
        {
            throw new AppException(HttpStatusCode.NotFound, String.Format(ErrorMessages.NOT_FOUND_ERROR, "Token", "Token", "Provided value"));
        }

        if (token.Expires > DateTime.UtcNow)
        {
            throw new AppException(HttpStatusCode.BadRequest, String.Format(ErrorMessages.BAD_REQUEST_FAILED, "refresh token while token is still valid"));
        }

        var readToken = _jwtHelper.ReadToken(token.Token);

        var userId = readToken.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault().Value;
        if (userId == null)
        {
            throw new AppException(HttpStatusCode.BadRequest, String.Format(ErrorMessages.BAD_REQUEST_INVALID, "Token"));
        }

        var user = await _dbSet.FindAsync(userId);
        if (user == null)
        {
            throw new AppException(HttpStatusCode.BadRequest, String.Format(ErrorMessages.BAD_REQUEST_INVALID, "Token"));
        }

        var result = await _jwtHelper.Create(user, DateTime.UtcNow.AddHours(1));
        await _whiteListedToken.AddAsync(result);
        _whiteListedToken.Remove(token);

        await _dbContext.SaveChangesAsync();


        return _mapper.Map<JWTTokenDTO>(result);
    }

    public async Task Logout(string token)
    {
        var existedToken = await _whiteListedToken.Where(x => x.Token == token).FirstOrDefaultAsync();
        if (existedToken == null)
        {
            throw new AppException(HttpStatusCode.NotFound, String.Format(ErrorMessages.NOT_FOUND_ERROR, "Token", "Token", "Provided value"));
        }

        _whiteListedToken.Remove(existedToken);

        await _dbContext.SaveChangesAsync();
    }
}