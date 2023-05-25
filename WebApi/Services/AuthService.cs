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
    Task<LoginResponseDTO> RefreshToken(RefreshTokenDTO refreshToken);
    Task<LoginResponseDTO> SignIn(UserLoginDTO userLogin);
    Task<UserDTO> SignUp(UserSignUpDTO userSignUp);
    Task ConfirmEmail(string confirmToken);
}

public class AuthService : BaseService<UserEntity>, IAuthService
{
    private readonly JWTHelper _jwtHelper;
    private readonly DbSet<JWTTokenEntity> _jwtDbSet;
    private readonly AppSettings _appSettings;
    public AuthService(ApplicationDbContext dbContext, JWTHelper jwtHelper, IMapper mapper, AppSettings appSettings) : base(dbContext, mapper)
    {
        _jwtHelper = jwtHelper;
        _jwtDbSet = dbContext.WhiteListedToken;
        _appSettings = appSettings;
    }
    public async Task<LoginResponseDTO> SignIn(UserLoginDTO userLogin)
    {
        var users = _dbSet.Include(x => x.UserRoles).ThenInclude(x => x.Role);
        var user = await users.Where(x => x.Email == userLogin.Email).FirstOrDefaultAsync();
        if (user == null)
        {
            throw new AppException(HttpStatusCode.NotFound, "User with this email does not exist");
        }

        if (!BCrypt.Net.BCrypt.Verify(userLogin.Password, user.Password, true))
        {
            throw new AppException(HttpStatusCode.BadRequest, "Password is incorrect");
        }

        var accessToken = _jwtHelper.Create(user, DateTime.UtcNow.AddMinutes(1));
        var refreshToken = _jwtHelper.Create(user, DateTime.UtcNow.AddMinutes(300));

        await _jwtDbSet.AddAsync(refreshToken);
        await _dbContext.SaveChangesAsync();

        var response = new LoginResponseDTO()
        {
            User = _mapper.Map<UserDTO>(user),
            AccessToken = accessToken.Token,
            RefreshToken = refreshToken.Token,
        };

        return response;
    }

    public async Task<UserDTO> SignUp(UserSignUpDTO userSignUp)
    {
        var user = _mapper.Map<UserEntity>(userSignUp);
        user.Cart = new();

        var duplicate = await _dbSet.Where(x => x.Email == userSignUp.Email).FirstOrDefaultAsync();
        if (duplicate != null)
        {
            throw new AppException(HttpStatusCode.Conflict, "Email already been used");
        }

        duplicate = await _dbSet.Where(x => x.UserInfo.PhoneNumber == userSignUp.PhoneNumber).FirstOrDefaultAsync();
        if (duplicate != null)
        {
            throw new AppException(HttpStatusCode.Conflict, "Phone number has already been used");
        }

        var role = await _dbContext.Roles.Where(x => x.Name == Roles.USER).FirstOrDefaultAsync();
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password, 11, true);
        user.UserRoles = new() {
            new UserRoleEntity{
                Role = role
            }
        };
        user.Cart = new()
        {
            CartProducts = new()
        };
        var res = await _dbSet.AddAsync(user);
        var emailConfirmToken = _jwtHelper.Create(res.Entity, DateTime.UtcNow.AddDays(7), TokenName.EmailConfirmToken);

        await _jwtDbSet.AddAsync(emailConfirmToken);

        var confirmationLink = $"{_appSettings.JWT.Audience}/confirmToken?={emailConfirmToken.Token}";
        Console.WriteLine(confirmationLink);

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<UserDTO>(user);
    }

    public async Task ConfirmEmail(string confirmToken)
    {
        var token = await _jwtHelper.AnalyzeToken(confirmToken);
        if (token == null)
        {
            throw new AppException(HttpStatusCode.BadRequest, "Invalid token");
        }

        var claim = token.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Name).FirstOrDefault();
        if (claim.Value != TokenName.EmailConfirmToken.ToString())
        {
            throw new AppException(HttpStatusCode.BadRequest, "Invalid token");
        }

        var tokenEntity = await _jwtDbSet.Where(x => x.Token == confirmToken).FirstOrDefaultAsync();
        if (tokenEntity == null)
        {
            throw new AppException(HttpStatusCode.BadRequest, "Invalid token");
        }

        var user = await _dbSet.FindAsync(token.Subject);
        if (user == null)
        {
            throw new AppException(HttpStatusCode.BadRequest, "Invalid token");
        }

        user.EmailConfirmed = true;
        _jwtDbSet.Remove(tokenEntity);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<LoginResponseDTO> RefreshToken(RefreshTokenDTO refreshTokenDTO)
    {
        var token = await _jwtHelper.AnalyzeToken(refreshTokenDTO.AccessToken);
        if (token != null)
        {
            throw new AppException(HttpStatusCode.BadRequest, "Can not get new refresh token while access token still valid");
        }

        token = await _jwtHelper.AnalyzeToken(refreshTokenDTO.RefreshToken);
        if (token.ValidTo < DateTime.UtcNow)
        {
            throw new AppException(HttpStatusCode.BadRequest, "Invalid refresh token");
        }

        var user = await _dbSet.Where(x => x.Id == token.Subject).FirstOrDefaultAsync();
        if (user == null)
        {
            throw new AppException(HttpStatusCode.BadRequest, "No user found is associated with token");
        }

        var refToken = await _jwtDbSet
                            .Include(x => x.User)
                            .Where(x => x.Token == refreshTokenDTO.RefreshToken && x.User.Id == user.Id)
                            .FirstOrDefaultAsync();

        if (refToken == null)
        {
            throw new AppException(HttpStatusCode.BadRequest, "Invalid refresh token");
        }

        _jwtDbSet.Remove(refToken);

        var refreshToken = _jwtHelper.Create(user, DateTime.UtcNow.AddMinutes(300));

        await _jwtDbSet.AddAsync(refreshToken);
        await _dbContext.SaveChangesAsync();

        return new()
        {
            User = _mapper.Map<UserDTO>(user),
            AccessToken = _jwtHelper.Create(user, DateTime.UtcNow.AddMinutes(1)).Token,
            RefreshToken = refreshToken.Token,
        };
    }

}