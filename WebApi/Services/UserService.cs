using System.Net;
using System.Text.RegularExpressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Contexts;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Helpers.JWT;
using WebApi.Middleware.ExceptionHandler;

namespace WebApi.Services;

public interface IUserService
{
    Task<PaginationResponseDTO<UserDTO>> Get(PaginationRequestDTO paginationRequest);
    Task<UserEntity> Get(string id);
    Task<UserDTO> Update(UserPatchDTO entity, string id);
    Task<string> Delete(string id);
}

public class UserService : BaseService<UserEntity>, IUserService
{
    public UserService(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<PaginationResponseDTO<UserDTO>> Get(PaginationRequestDTO paginationRequest)
    {
        paginationRequest.PerPage = paginationRequest.PerPage == 0 ? 20 : paginationRequest.PerPage;
        var users = await _dbSet.Include(x => x.UserInfo).ToListAsync();

        var items = _mapper.Map<List<UserDTO>>(users);
        var res = new PaginationResponseDTO<UserDTO>(paginationRequest, items);

        return res;
    }

    public async Task<UserEntity> Get(string id)
    {
        var user = await _dbSet.FindAsync(id);

        if (user == null)
        {
            throw new AppException(HttpStatusCode.NotFound, $"Cannot find user with id: {id}");
        }

        return user;
    }

    public async Task<string> Delete(string id)
    {
        var user = await _dbSet.FindAsync(id);

        if (user == null)
        {
            throw new AppException(HttpStatusCode.NotFound, $"Cannot find user with id: {id}");
        }

        _dbContext.Remove(user);
        await _dbContext.SaveChangesAsync();
        return id;
    }

    public async Task<UserDTO> Update(UserPatchDTO entity, string id)
    {
        var user = await _dbSet.FindAsync(id);
        if (user == null)
        {
            throw new AppException(HttpStatusCode.NotFound, "Cannot find user with provided id");
        }

        var validate = _dbSet.Where(x => (x.UserInfo.PhoneNumber == entity.PhoneNumber || x.Email == entity.Email) && x.Id != user.Id).ToList();
        if (validate.Count() > 0)
        {
            throw new AppException(HttpStatusCode.Conflict, $"Already user with this credential");
        }

        foreach (var property in typeof(UserPatchDTO).GetProperties())
        {
            var value = property.GetValue(entity);
            if (property.GetValue(user) != null && value != null)
            {
                property.SetValue(user, value, null);
            }
            else if (property.GetValue(user.UserInfo) != null && value != null)
            {
                property.SetValue(user.UserInfo, value, null);
            }
        }

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<UserDTO>(user);
    }
}