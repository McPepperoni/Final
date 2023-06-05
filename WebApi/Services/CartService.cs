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
    Task<CartDTO> Get();
    Task<CartDTO> Update(UpdateCartDTO updateCart);
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

    public async Task<CartDTO> Get()
    {
        var user = await _userDbSet
        .Where(x => x.Id.ToString() == _userId)
        .Include(x => x.Cart)
        .ThenInclude(x => x.CartProducts)
        .ThenInclude(x => x.Product)
        .FirstOrDefaultAsync();

        if (user.Cart == null)
        {
            throw new AppException(HttpStatusCode.NotFound, String.Format(ErrorMessages.NOT_FOUND_ERROR, "Cart", "UserId", _userId));
        }

        if (user.Cart.CartProducts == null)
        {
            user.Cart.CartProducts = new() { };
        }
        return _mapper.Map<CartDTO>(user.Cart);
    }

    public async Task<CartDTO> Update(UpdateCartDTO updateCart)
    {
        var user = await _userDbSet
        .Where(x => x.Id.ToString() == _userId)
        .Include(x => x.Cart)
        .ThenInclude(x => x.CartProducts)
        .ThenInclude(x => x.Product)
        .FirstOrDefaultAsync();

        var cart = user.Cart;
        if (cart == null)
        {
            throw new AppException(HttpStatusCode.NotFound, String.Format(ErrorMessages.NOT_FOUND_ERROR, "Cart", "UserId", _userId));
        }

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
}
