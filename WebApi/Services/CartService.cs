using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Entities;
using WebApi.DTOs;
using WebApi.Middleware.ExceptionHandler;

namespace WebApi.Services;

public interface ICartService
{
    Task<CartDTO> Create(string userId);
    Task<CartDTO> Get(string userId);
}

public class CartService : BaseService<CartEntity>, ICartService
{
    public CartService(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {

    }

    public async Task<CartDTO> Create(string userId)
    {
        throw new NotImplementedException();

    }

    public async Task<CartDTO> Get(string userId)
    {
        throw new NotImplementedException();
    }
}
 