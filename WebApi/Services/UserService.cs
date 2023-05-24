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
    Task<SearchResponseUserDTO> Search(SearchRequestUserDTO searchRequest);
    Task<UserDTO> Update(UserPatchDTO entity, string id);
    Task<string> Delete(string id);
    Task<ListDTO<string>> DeleteAll();
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

    public async Task<ListDTO<string>> DeleteAll()
    {
        var users = _dbSet.ToList();
        var res = new ListDTO<string>();

        foreach (var item in users)
        {
            _dbSet.Remove(item);
            res.Items.Add(item.Id);
        }

        await _dbContext.SaveChangesAsync();
        return res;
    }

    public async Task<SearchResponseUserDTO> Search(SearchRequestUserDTO searchRequest)
    {
        var byEmailResult = new SearchResultDTO<List<UserDTO>>();
        var byNameResult = new SearchResultDTO<List<UserDTO>>();

        if (searchRequest.IncludeEmail)
        {
            var byEmail = await _dbSet.Include(x => x.UserInfo).Where(x => EF.Functions.Like(x.Email.ToLower(), $"%{searchRequest.Query.ToLower()}%"))
                                .ToListAsync();

            byEmailResult = new SearchResultDTO<List<UserDTO>>()
            {
                Total = byEmail.Count,
                Data = _mapper.Map<List<UserDTO>>(byEmail.Take(searchRequest.Limit).ToList()),
            };
        }

        if (searchRequest.IncludeEmail)
        {
            var byName = (await _dbSet.Include(x => x.UserInfo)
                                .Where(x => EF.Functions.Like(x.UserInfo.FirstName.ToLower(), $"%{searchRequest.Query.ToLower()}%") ||
                                            EF.Functions.Like(x.UserInfo.LastName.ToLower(), $"%{searchRequest.Query.ToLower()}%"))
                                .ToListAsync())
                                .Take(searchRequest.Limit)
                                .ToList();

            byNameResult = new SearchResultDTO<List<UserDTO>>()
            {
                Total = byName.Count,
                Data = _mapper.Map<List<UserDTO>>(byName.Take(searchRequest.Limit).ToList()),
            };
        }

        return new()
        {
            SearchParams = searchRequest,
            Result = new()
            {
                ByEmail = byEmailResult,
                ByName = byNameResult
            }
        };
    }
}