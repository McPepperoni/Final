using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Contexts;
using WebApi.DTOs;
using WebApi.Entities;
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
        var cart = await _dbSet.Where(x => x.UserId == userId).FirstOrDefaultAsync();
        if (cart != null)
        {
            throw new AppException(HttpStatusCode.Conflict, "Cart associated with this user has already exist");
        }

        cart = new()
        {
            UserId = userId,
        };

        await _dbSet.AddAsync(cart);

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<CartDTO>(cart);
    }

    public async Task<CartDTO> Get(string userId)
    {
        var cart = await _dbSet.Where(x => x.UserId == userId).Include(x => x.CartProducts).ThenInclude(x => x.Product).FirstOrDefaultAsync();
        if (cart != null)
        {
            throw new AppException(HttpStatusCode.NotFound, "No cart associated with this user");
        }

        return _mapper.Map<CartDTO>(cart);
    }
}