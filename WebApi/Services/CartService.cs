using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Entities;
using WebApi.Constants;
using WebApi.DTOs.CartDTO;
using WebApi.Middleware.ExceptionHandler;

namespace WebApi.Services;

public interface ICartService
{
    Task<CartDetailDTO> Get();
    Task<CartDetailDTO> Update(UpdateCartDTO updateCart);
    Task<CartDetailDTO> AddToCart(AddToCartDTO addToCart);
    Task<CartDetailDTO> RemoveFromCart(string itemId);
}

public class CartService : BaseService, ICartService
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

    public async Task<CartDetailDTO> Get()
    {
        var cart = await _dbContext.Carts
                        .Where(x => x.UserId == _userId)
                        .Include(x => x.CartProducts)
                        .ThenInclude(x => x.Product)
                        .FirstOrDefaultAsync();

        if (cart == null)
        {
            throw new AppException(HttpStatusCode.NotFound, string.Format(ErrorMessages.NOT_FOUND_ERROR, "Cart", "UserId", _userId));
        }

        if (cart.CartProducts == null)
        {
            cart.CartProducts = new() { };
        }
        return _mapper.Map<CartDetailDTO>(cart);
    }

    public async Task<CartDetailDTO> Update(UpdateCartDTO updateCart)
    {
        var cart = await _dbContext.Carts
                        .Where(x => x.UserId == _userId)
                        .Include(x => x.CartProducts)
                        .ThenInclude(x => x.Product)
                        .FirstOrDefaultAsync();

        if (cart == null)
        {
            throw new AppException(HttpStatusCode.NotFound, string.Format(ErrorMessages.NOT_FOUND_ERROR, "Cart", "UserId", _userId));
        }

        foreach (var item in updateCart.Instruction)
        {
            var product = await _productDbSet.FindAsync(item.ProductId);
            if (product == null)
            {
                throw new AppException(HttpStatusCode.BadRequest, string.Format(ErrorMessages.NOT_FOUND_ERROR, "Product", "id", item.ProductId));
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

        return _mapper.Map<CartDetailDTO>(cart);
    }

    public async Task<CartDetailDTO> AddToCart(AddToCartDTO addToCart)
    {
        var cart = _dbContext.Carts.FirstOrDefault(x => x.UserId == _userId);

        if (cart == null)
        {
            throw new AppException(HttpStatusCode.NotFound, string.Format(ErrorMessages.NOT_FOUND_ERROR, "User", "Id", _userId));
        }

        var product = await _productDbSet
                        .FindAsync(addToCart.ItemId);

        if (product == null)
        {
            throw new AppException(HttpStatusCode.NotFound, string.Format(ErrorMessages.NOT_FOUND_ERROR, "Product", "Id", addToCart.ItemId));
        }

        if (product.Quantity < addToCart.Quantity)
        {
            throw new AppException(HttpStatusCode.BadRequest, string.Format(ErrorMessages.BAD_REQUEST_INVALID, "Product Quantity"));
        }

        if (cart.CartProducts == null)
        {
            var cartProduct = new List<CartProductEntity>();
            var item = new CartProductEntity
            {
                Product = product,
                Quantity = addToCart.Quantity
            };

            cartProduct.Add(item);
            cart.CartProducts = cartProduct;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<CartDetailDTO>(cart);
        }

        var existedItem = cart.CartProducts.Where(x => x.Product.Id == addToCart.ItemId).FirstOrDefault();

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


        cart.CartProducts.Add(existedItem);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<CartDetailDTO>(cart);
    }

    public async Task<CartDetailDTO> RemoveFromCart(string id)
    {
        var item = await _cartProductDbSet.FindAsync(id);
        if (item == null)
        {
            throw new AppException(HttpStatusCode.NotFound, string.Format(ErrorMessages.NOT_FOUND_ERROR, "Cart item", "Id", id));
        }

        _cartProductDbSet.Remove(item);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<CartDetailDTO>(item.Cart);
    }
}
