using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Entities;
using WebApi.Constants;
using WebApi.DTOs;
using WebApi.Middleware.ExceptionHandler;

namespace WebApi.Services;

public interface IOrderService
{
    Task<OrderDTO> Create(CreateOrderDTO createOrder);
    Task ProceedOrder(string id);
    Task CancelOrder(string id);
}

public class OrderService : BaseService<OrderEntity>, IOrderService
{
    private readonly DbSet<UserEntity> _users;
    private readonly DbSet<CartProductEntity> _cartProducts;
    private readonly DbSet<OrderProductEntity> _orderProducts;

    public OrderService(ApplicationDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor, dbContext, mapper)
    {
        _cartProducts = dbContext.CartProducts;
        _orderProducts = dbContext.OrderProducts;
        _users = dbContext.Users;
    }

    public async Task<OrderDTO> Create(CreateOrderDTO createOrder)
    {
        var user = await _users
                            .Include(x => x.Cart)
                            .ThenInclude(x => x.CartProducts)
                            .ThenInclude(x => x.Product)
                            .Where(x => x.Id.ToString() == _userId)
                            .FirstOrDefaultAsync();

        if (user == null)
        {
            throw new AppException(HttpStatusCode.NotFound, String.Format(ErrorMessages.NOT_FOUND_ERROR, "User", "id", _userId));
        }

        var cartProductsId = new List<string>();
        foreach (var item in user.Cart.CartProducts)
        {
            cartProductsId.Add(item.Product.Id);
        }

        var orderProducts = new List<OrderProductEntity>();
        foreach (var item in createOrder.Products)
        {
            if (!cartProductsId.Contains(item.ProductId))
            {
                throw new AppException(HttpStatusCode.BadRequest, String.Format(ErrorMessages.DOES_NOT_MATCH, "CartProduct", "OrderProduct"));
            }

            var cartProduct = user.Cart.CartProducts
                            .Where(x => x.Product.Id == item.ProductId)
                            .FirstOrDefault();

            var orderProduct = new OrderProductEntity
            {
                Quantity = item.Quantity,
                Product = cartProduct.Product,
            };

            _cartProducts.Remove(cartProduct);
            orderProducts.Add(orderProduct);
        }

        var order = new OrderEntity()
        {
            User = user,
            OrderProducts = orderProducts,
        };

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<OrderDTO>(order);
    }

    public async Task CancelOrder(string id)
    {
        var order = await _dbSet.FindAsync(id);

        if (order == null)
        {
            throw new AppException(HttpStatusCode.NotFound, String.Format(ErrorMessages.NOT_FOUND_ERROR, "Order", "Id", id));
        }

        _dbSet.Remove(order);
        await _dbContext.SaveChangesAsync();
    }

    public async Task ProceedOrder(string id)
    {
        var order = await _dbSet.FindAsync(id);
        if (order == null)
        {
            throw new AppException(HttpStatusCode.NotFound, String.Format(ErrorMessages.NOT_FOUND_ERROR, "Order", "Id", id));
        }

        order.DeliveringStatus = true;

        await _dbContext.SaveChangesAsync();
    }
}