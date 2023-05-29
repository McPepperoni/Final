using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace WebApi.Services;

public abstract class BaseService<T> where T : class
{
    protected readonly ApplicationDbContext _dbContext;
    protected readonly DbSet<T> _dbSet;
    protected readonly IMapper _mapper;

    public BaseService(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<T>();
        _mapper = mapper;
    }
}