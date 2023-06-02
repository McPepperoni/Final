using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Entities;
using WebApi.Constants;
using WebApi.DTOs;
using WebApi.Middleware.ExceptionHandler;

namespace WebApi.Services;

public interface ICartService
{
    Task CreateAsync(string userId);
    Task<CartDTO> Get(string userId);
    Task<CartDTO> Update(UpdateCartDTO updateCart);
    Task<CartDTO> Delete(string itemId);

}

public class CartService : BaseService<CartEntity>, ICartService
{
    private readonly DbSet<UserEntity> _userDbSet;
    private readonly DbSet<ProductEntity> _productDbSet;
    private readonly DbSet<CartProductEntity> _cartProductDbSet;
    public CartService(ApplicationDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor, dbContext, mapper)
    {
        _userDbSet = dbContext.Users;
        _productDbSet = dbContext.Products;
        _cartProductDbSet = dbContext.CartProducts;
    }

    public async Task CreateAsync(string userId)
    {
        var user = await _userDbSet.FindAsync(userId);

        if (user.Cart != null)
        {
            throw new AppException(HttpStatusCode.Conflict, String.Format(ErrorMessages.CONFLICTED_ERROR, "Cart", "UserId", userId));
        }

        user.Cart = new CartEntity() { };

        await _dbContext.SaveChangesAsync();
    }

    public async Task<CartDTO> Get(string userId)
    {
        var user = await _userDbSet
        .Where(x => x.Id.ToString() == userId)
        .Include(x => x.Cart)
        .ThenInclude(x => x.CartProducts)
        .ThenInclude(x => x.Product)
        .FirstOrDefaultAsync();

        if (user.Cart == null)
        {
            throw new AppException(HttpStatusCode.NotFound, String.Format(ErrorMessages.NOT_FOUND_ERROR, "Cart", "UserId", userId));
        }

        if (user.Cart.CartProducts == null)
        {
            user.Cart.CartProducts = new() { };
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
                throw new AppException(HttpStatusCode.BadRequest, String.Format(ErrorMessages.NOT_FOUND_ERROR, "Product", "id", item.ProductId.ToString()));
            }

            var productInCart = cart.CartProducts.Find(x => x.Product.Id == item.ProductId);
            if (productInCart == null && item.Quantity > 0)
            {
                cart.CartProducts.Add(new CartProductEntity
                {
                    Quantity = item.Quantity,
                    Product = product,
                });
            }

            productInCart.Quantity = item.Quantity;

            if (item.Quantity == 0)
            {
                _cartProductDbSet.Remove(productInCart);
            }
        }

        return _mapper.Map<CartDTO>(cart);
    }

    public async Task<CartDTO> Delete(string itemId)
    {
        var cartItem = await _cartProductDbSet.FindAsync(itemId);

        if (cartItem == null)
        {
            throw new AppException(HttpStatusCode.NotFound, String.Format(ErrorMessages.NOT_FOUND_ERROR, "Cart Item", "Id", itemId));
        }
        var cart = cartItem.Cart;

        _cartProductDbSet.Remove(cartItem);

        return _mapper.Map<CartEntity, CartDTO>(cart);
    }
}
