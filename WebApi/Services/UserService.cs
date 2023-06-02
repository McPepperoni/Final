using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Entities;
using Persistence.Managers;
using WebApi.Constants;
using WebApi.DTOs;
using WebApi.Middleware.ExceptionHandler;

namespace WebApi.Services;

public interface IUserService
{
    Task<List<UserDTO>> Get();
    Task<UserDTO> Get(string id);
    Task Create(CreateUserDTO createUser);
    Task Update(string id, UpdateUserDTO updateUser);
    Task Delete(string Id);
}

public class UserService : BaseService<UserEntity>, IUserService
{
    private readonly FinalUserManager _userManager;
    public UserService(ApplicationDbContext dbContext, IMapper mapper, FinalUserManager userManager, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor, dbContext, mapper)
    {
        _userManager = userManager;
    }

    public async Task<List<UserDTO>> Get()
    {
        return _mapper.Map<List<UserDTO>>(await _dbSet.ToListAsync());
    }

    public async Task<UserDTO> Get(string id)
    {
        var user = await _dbSet.FindAsync(id);

        if (user == null)
        {
            throw new AppException(HttpStatusCode.NotFound, String.Format(ErrorMessages.NOT_FOUND_ERROR, "User", "Id", id));
        }

        return _mapper.Map<UserDTO>(user);
    }

    public async Task Create(CreateUserDTO createUser)
    {
        var user = _mapper.Map<UserEntity>(createUser);

        var result = await _userManager.CreateAsync(user, createUser.Password);

        if (!result.Succeeded)
        {
            throw new AppException(HttpStatusCode.BadRequest, "Registered failed with messages");
        }
    }
    public async Task Update(string id, UpdateUserDTO updateUser)
    {
        var user = await _dbSet.FindAsync(id);
        if (user == null)
        {
            throw new AppException(HttpStatusCode.NotFound, String.Format(ErrorMessages.NOT_FOUND_ERROR, "User", "id", id));
        }

        user.Email = updateUser.Email;
        user.FullName = updateUser.FullName;
        user.PhoneNumber = updateUser.PhoneNumber;
        user.Address = updateUser.Address;

        await _dbContext.SaveChangesAsync();
    }
    public async Task Delete(string Id)
    {
        var user = await _dbSet.FindAsync(Id);
        if (user == null)
        {
            throw new AppException(HttpStatusCode.NotFound, String.Format(ErrorMessages.NOT_FOUND_ERROR, "User", "Id", Id));
        }

        _dbSet.Remove(user);

        await _dbContext.SaveChangesAsync();
    }
}