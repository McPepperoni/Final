using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Contexts;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Helpers.JWT;
using WebApi.Middleware.ExceptionHandler;
using WebApi.Services;

public interface IAuthService
{
    Task<LoginResponseDTO> RefreshToken(RefreshTokenDTO refreshToken);
    Task<LoginResponseDTO> SignIn(UserLoginDTO userLogin);
    Task<UserDTO> SignUp(UserSignUpDTO userSignUp);

}

public class AuthService : BaseService<UserEntity>, IAuthService
{
    private readonly JWTHelper _jwtHelper;
    private readonly DbSet<JWTTokenEntity> _jwtDbSet;
    public AuthService(ApplicationDbContext dbContext, JWTHelper jwtHelper, IMapper mapper) : base(dbContext, mapper)
    {
        _jwtHelper = jwtHelper;
        _jwtDbSet = dbContext.WhiteListedToken;
    }
    public async Task<LoginResponseDTO> SignIn(UserLoginDTO userLogin)
    {
        var user = await _dbSet.Where(x => x.Email == userLogin.Email).FirstOrDefaultAsync();
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

        var res = await _dbSet.AddAsync(user);

        if (user == null)
        {
            throw new AppException(HttpStatusCode.Conflict, "User with that info already existed");
        }

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<UserDTO>(user);
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