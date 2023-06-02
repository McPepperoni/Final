using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace WebApi.Services;

public abstract class BaseService<T> where T : class
{
    protected readonly ApplicationDbContext _dbContext;
    protected readonly DbSet<T> _dbSet;
    protected readonly IMapper _mapper;
    protected readonly string _userId;

    public BaseService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<T>();
        _mapper = mapper;

        _userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}