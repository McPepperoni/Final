using System.Security.Claims;
using AutoMapper;
using Persistence;

namespace WebApi.Services;

public abstract class BaseService
{
    protected readonly ApplicationDbContext _dbContext;
    protected readonly IMapper _mapper;
    protected readonly string _userId;

    public BaseService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}