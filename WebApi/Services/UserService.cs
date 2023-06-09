using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Entities;
using Persistence.Managers;
using WebApi.Constants;
using WebApi.DTOs.UserDTO;
using WebApi.Middleware.ExceptionHandler;

namespace WebApi.Services;

public interface IUserService
{
    Task<List<UserDetailDTO>> Get();
    Task<UserDetailDTO> Get(string id);
    Task Create(CreateUserDTO createUser);
    Task Update(string id, UpdateUserDTO updateUser);
    Task Delete(string Id);
}

public class UserService : BaseService, IUserService
{
    private readonly FinalUserManager _userManager;
    public UserService(ApplicationDbContext dbContext, IMapper mapper, FinalUserManager userManager, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor, dbContext, mapper)
    {
        _userManager = userManager;
    }

    public async Task<List<UserDetailDTO>> Get()
    {
        return _mapper.Map<List<UserDetailDTO>>(await _dbContext.Users.ToListAsync());
    }

    public async Task<UserDetailDTO> Get(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            throw new AppException(HttpStatusCode.NotFound, string.Format(ErrorMessages.NOT_FOUND_ERROR, "User", "Id", id));
        }

        return _mapper.Map<UserDetailDTO>(user);
    }

    public async Task Create(CreateUserDTO createUser)
    {
        var existedUser = await _userManager.FindByEmailAsync(createUser.Email);
        if (existedUser != null)
        {
            throw new AppException(HttpStatusCode.Conflict, string.Format(ErrorMessages.CONFLICTED_ERROR, "User", "Email", createUser.Email));
        }

        var user = _mapper.Map<UserEntity>(createUser);
        var result = await _userManager.CreateAsync(user, createUser.Password);
        if (!result.Succeeded)
        {
            throw new AppException(HttpStatusCode.BadRequest, string.Join(",", result.Errors));
        }

        result = await _userManager.AddToRoleAsync(user, "User");

        if (!result.Succeeded)
        {
            throw new AppException(HttpStatusCode.BadRequest, string.Join(",", result.Errors));
        }
    }
    public async Task Update(string id, UpdateUserDTO updateUser)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            throw new AppException(HttpStatusCode.NotFound, string.Format(ErrorMessages.NOT_FOUND_ERROR, "User", "id", id));
        }

        user.Email = updateUser.Email;
        user.FullName = updateUser.FullName;
        user.PhoneNumber = updateUser.PhoneNumber;
        user.Address = updateUser.Address;

        await _userManager.UpdateAsync(user);
    }
    public async Task Delete(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            throw new AppException(HttpStatusCode.NotFound, string.Format(ErrorMessages.NOT_FOUND_ERROR, "User", "Id", id));
        }

        await _userManager.DeleteAsync(user);
    }
}