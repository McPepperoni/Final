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
    Task Create(string userId);
    Task<CartDTO> Get(string userId);
    Task<CartDTO> Update(UpdateCartDTO updateCart);
}

public class CartService : BaseService<CartEntity>, ICartService
{
    private readonly DbSet<UserEntity> _userDbSet;
    private readonly DbSet<ProductEntity> _productDbSet;
    public CartService(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {

    }

    public async Task Create(string userId)
    {
        var user = await _userDbSet.FindAsync(userId);

        if (user.Cart != null)
        {
            throw new AppException(HttpStatusCode.Conflict, "User already has cart");
        }

        user.Cart = new CartEntity() { };
    }

    public async Task<CartDTO> Get(string userId)
    {
        var user = await _userDbSet.FindAsync(userId);

        if (user.Cart == null)
        {
            throw new AppException(HttpStatusCode.Conflict, "User does not has cart");
        }

        return _mapper.Map<CartDTO>(user.Cart);
    }

    public async Task<CartDTO> Update(UpdateCartDTO updateCart)
    {
        var cart = await _dbSet.FindAsync(updateCart.Id);

        foreach (var item in updateCart.Instruction)
        {
            var product = await _productDbSet.FindAsync(item.ProductId);
            if (product == null)
            {
                throw new AppException(HttpStatusCode.BadRequest, "Product provided not exist");
            }

            var productInCart = cart.CartProducts.Find(x => x.Product.Id == item.ProductId);
            if (productInCart == null) { }
        }


        return _mapper.Map<CartDTO>(cart);
    }
}
