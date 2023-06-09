using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Managers;
using WebApi.Constants;
using WebApi.DTOs.AuthDTO;
using WebApi.Helpers.JWT;
using WebApi.Middleware.ExceptionHandler;
using WebApi.Services;
using WebApi.Settings;
public interface IAuthService
{
    Task<ResultAuthDTO> Login(LoginAuthDTO authLogin);
    Task Logout(RefreshTokenAuth token);
    Task<ResultAuthDTO> RefreshToken(RefreshTokenAuth token);
}

public class AuthService : BaseService, IAuthService
{
    private readonly JWTHelper _jwtHelper;
    private readonly AppSettings _appSettings;
    private readonly FinalUserManager _userManager;
    private readonly FinalSignInManager _signInManager;
    public AuthService(ApplicationDbContext dbContext, JWTHelper jwtHelper, IMapper mapper, AppSettings appSettings, FinalUserManager userManager, FinalSignInManager signInManager, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor, dbContext, mapper)
    {
        _jwtHelper = jwtHelper;
        _appSettings = appSettings;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<ResultAuthDTO> Login(LoginAuthDTO authLogin)
    {
        var result = await _signInManager.SignInWithEmailPasswordAsync(authLogin.Email, authLogin.Password, true);

        if (!result.Succeeded)
        {
            throw new AppException(HttpStatusCode.BadRequest, string.Format(ErrorMessages.BAD_REQUEST_FAILED, "Login"));
        }

        var accessToken = await _jwtHelper.Create(await _userManager.FindByEmailAsync(authLogin.Email), DateTime.UtcNow.AddHours(1));
        var refreshToken = await _jwtHelper.Create(await _userManager.FindByEmailAsync(authLogin.Email), DateTime.UtcNow.AddDays(7));
        await _dbContext.WhiteListedToken.AddAsync(refreshToken);

        await _dbContext.SaveChangesAsync();




        return new ResultAuthDTO
        {
            AccessToken = accessToken.Token,
            RefreshToken = refreshToken.Token
        };
    }

    public async Task<ResultAuthDTO> RefreshToken(RefreshTokenAuth ExpiredToken)
    {
        var token = await _dbContext.WhiteListedToken.Where(x => x.Token == ExpiredToken.RefreshToken).FirstOrDefaultAsync();
        if (token == null)
        {
            throw new AppException(HttpStatusCode.NotFound, string.Format(ErrorMessages.NOT_FOUND_ERROR, "Token", "Token", "Provided value"));
        }

        if (token.Expires > DateTime.UtcNow)
        {
            throw new AppException(HttpStatusCode.BadRequest, string.Format(ErrorMessages.BAD_REQUEST_FAILED, "refresh token while token is still valid"));
        }

        var user = await _userManager.FindByIdAsync(_userId);
        if (user == null)
        {
            throw new AppException(HttpStatusCode.BadRequest, string.Format(ErrorMessages.BAD_REQUEST_INVALID, "Token"));
        }

        var refreshToken = await _jwtHelper.Create(user, DateTime.UtcNow.AddHours(1));
        var accessToken = await _jwtHelper.Create(user, DateTime.UtcNow.AddDays(7));

        await _dbContext.WhiteListedToken.AddAsync(refreshToken);
        _dbContext.WhiteListedToken.Remove(token);

        await _dbContext.SaveChangesAsync();


        return new ResultAuthDTO
        {
            AccessToken = accessToken.Token,
            RefreshToken = refreshToken.Token
        };
    }

    public async Task Logout(RefreshTokenAuth token)
    {
        var existedToken = await _dbContext.WhiteListedToken.Where(x => x.Token == token.RefreshToken).FirstOrDefaultAsync();
        if (existedToken == null)
        {
            throw new AppException(HttpStatusCode.NotFound, string.Format(ErrorMessages.NOT_FOUND_ERROR, "Token", "Token", "Provided value"));
        }

        _dbContext.WhiteListedToken.Remove(existedToken);

        await _dbContext.SaveChangesAsync();
    }
}