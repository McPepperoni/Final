using System.Net;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Persistence;
using Persistence.Entities;
using Persistence.Managers;
using WebApi.DTOs;
using WebApi.Helpers.JWT;
using WebApi.Middleware.ExceptionHandler;
using WebApi.Services;
using WebApi.Settings;
public interface IAuthService
{
    Task Register(AuthRegisterDTO authRegister);
    Task<JWTTokenEntity> Login(AuthLoginDTO authLogin);
    Task Logout();
}

public class AuthService : BaseService<UserEntity>, IAuthService
{
    private readonly JWTHelper _jwtHelper;
    private readonly AppSettings _appSettings;
    private readonly FinalUserManager _userManager;
    private readonly FinalSignInManager _signInManager;
    public AuthService(ApplicationDbContext dbContext, JWTHelper jwtHelper, IMapper mapper, AppSettings appSettings, FinalUserManager userManager, FinalSignInManager signInManager) : base(dbContext, mapper)
    {
        _jwtHelper = jwtHelper;
        _appSettings = appSettings;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task Register(AuthRegisterDTO authRegister)
    {
        var user = _mapper.Map<UserEntity>(authRegister);

        var result = await _userManager.CreateAsync(user, authRegister.Password);

        if (!result.Succeeded)
        {
            throw new AppException(HttpStatusCode.BadRequest, "Registered failed with messages");
        }
    }

    public async Task<JWTTokenEntity> Login(AuthLoginDTO authLogin)
    {
        var result = await _signInManager.SignInWithEmailPasswordAsync(authLogin.Email, authLogin.Password, true);

        if (!result.Succeeded)
        {
            throw new AppException(HttpStatusCode.BadRequest);
        }

        var token = await _jwtHelper.Create(await _userManager.FindByEmailAsync(authLogin.Email), DateTime.UtcNow.AddHours(1));

        return token;
    }

    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }
}