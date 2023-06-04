using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Entities;
using WebApi.Constants;
using WebApi.DTOs;
using WebApi.Middleware.ExceptionHandler;

namespace WebApi.Services;

public interface ICartItemService
{
    Task<CartDTO> RemoveFromCart(string id);
    Task<CartDTO> AddToCart(AddToCartDTO addToCart);
}

public class CartItemService : BaseService<CartEntity>, ICartItemService
{
    private readonly DbSet<UserEntity> _userDbSet;
    private readonly DbSet<ProductEntity> _productDbSet;
    private readonly DbSet<CartProductEntity> _cartProductDbSet;
    public CartItemService(ApplicationDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor, dbContext, mapper)
    {
        _userDbSet = dbContext.Users;
        _productDbSet = dbContext.Products;
        _cartProductDbSet = dbContext.CartProducts;
    }

    public async Task<CartDTO> RemoveFromCart(string id)
    {
        var item = await _cartProductDbSet.FindAsync(id);
        if (item == null)
        {
            throw new AppException(HttpStatusCode.NotFound, String.Format(ErrorMessages.NOT_FOUND_ERROR, "Cart item", "Id", id));
        }

        _cartProductDbSet.Remove(item);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<CartDTO>(item.Cart);
    }

    public async Task<CartDTO> AddToCart(AddToCartDTO addToCart)
    {
        var user = await _userDbSet
                        .Include(x => x.Cart)
                        .ThenInclude(x => x.CartProducts)
                        .ThenInclude(x => x.Product)
                        .Where(x => x.Id.ToString() == _userId)
                        .FirstOrDefaultAsync();

        if (user == null)
        {
            throw new AppException(HttpStatusCode.NotFound, String.Format(ErrorMessages.NOT_FOUND_ERROR, "User", "Id", _userId));
        }

        var product = await _productDbSet
                        .FindAsync(addToCart.ItemId);

        if (product == null)
        {
            throw new AppException(HttpStatusCode.NotFound, String.Format(ErrorMessages.NOT_FOUND_ERROR, "Product", "Id", addToCart.ItemId));
        }

        if (product.Quantity < addToCart.Quantity)
        {
            throw new AppException(HttpStatusCode.BadRequest, String.Format(ErrorMessages.BAD_REQUEST_INVALID, "Product Quantity"));
        }

        if (user.Cart.CartProducts == null)
        {
            var cartProduct = new List<CartProductEntity>();
            var item = new CartProductEntity
            {
                Product = product,
                Quantity = addToCart.Quantity
            };

            cartProduct.Add(item);
            user.Cart.CartProducts = cartProduct;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<CartDTO>(user.Cart);
        }

        var existedItem = user.Cart.CartProducts.Where(x => x.Product.Id == addToCart.ItemId).FirstOrDefault();

        if (existedItem == null)
        {
            existedItem = new CartProductEntity()
            {
                Product = product,
                Quantity = addToCart.Quantity
            };
        }
        else
        {
            existedItem.Quantity += addToCart.Quantity;
        }


        user.Cart.CartProducts.Add(existedItem);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<CartDTO>(user.Cart);
    }
}
