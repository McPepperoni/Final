using System.Collections.Generic;
using System.Data;
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
    Task<OrderDTO> CreateAsync(CreateOrderDTO createOrder);
    Task UpdateOrder(string id, string orderStatus);
    Task CancelOrderAsync(string id);
    Task<List<OrderDTO>> Filter(string orderState);
    Task<OrderDTO> Get(string id);
}

public class OrderService : BaseService<OrderEntity>, IOrderService
{
    private readonly DbSet<CartProductEntity> _cartProducts;
    private readonly DbSet<OrderProductEntity> _orderProducts;
    private readonly DbSet<CartEntity> _cartDbSet;

    public OrderService(ApplicationDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor, dbContext, mapper)
    {
        _cartProducts = dbContext.CartProducts;
        _orderProducts = dbContext.OrderProducts;
        _cartDbSet = dbContext.Carts;
    }

    public async Task<OrderDTO> CreateAsync(CreateOrderDTO createOrder)
    {
        var cart = await _cartDbSet
                            .Where(x => x.UserId == _userId)
                            .Include(x => x.CartProducts)
                            .ThenInclude(x => x.Product)
                            .FirstOrDefaultAsync();

        if (cart == null)
        {
            throw new AppException(HttpStatusCode.NotFound, String.Format(ErrorMessages.NOT_FOUND_ERROR, "User", "id", _userId));
        }

        var cartProductsId = new List<string>();
        foreach (var item in cart.CartProducts)
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

            var cartProduct = cart.CartProducts
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
            UserId = cart.UserId,
            Products = orderProducts,
        };
        order.Status = OrderStatus.WAIT_FOR_APPROVAL;

        await _dbSet.AddAsync(order);

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<OrderDTO>(order);
    }

    public async Task CancelOrderAsync(string id)
    {
        var order = await _dbSet.FindAsync(id);

        if (order == null)
        {
            throw new AppException(HttpStatusCode.NotFound, String.Format(ErrorMessages.NOT_FOUND_ERROR, "Order", "Id", id));
        }

        order.Status = OrderStatus.CANCELED;
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateOrder(string id, string orderState)
    {
        if ((orderState != OrderStatus.CANCELED && orderState != OrderStatus.DELIVERED && orderState != OrderStatus.DELIVERING && orderState != OrderStatus.WAIT_FOR_APPROVAL) || orderState == OrderStatus.CANCELED)
        {
            throw new AppException(HttpStatusCode.BadRequest, String.Format(ErrorMessages.BAD_REQUEST_INVALID, "Order State"));
        }

        var order = await _dbSet.FindAsync(id);
        if (order == null)
        {
            throw new AppException(HttpStatusCode.NotFound, String.Format(ErrorMessages.NOT_FOUND_ERROR, "Order", "Id", id));
        }

        order.Status = orderState;

        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<OrderDTO>> Filter(string orderState = OrderStatus.WAIT_FOR_APPROVAL)
    {
        if (orderState != OrderStatus.CANCELED && orderState != OrderStatus.DELIVERED && orderState != OrderStatus.DELIVERING && orderState != OrderStatus.WAIT_FOR_APPROVAL)
        {
            throw new AppException(HttpStatusCode.BadRequest, String.Format(ErrorMessages.BAD_REQUEST_INVALID, "Order State"));
        }

        var orders = await _dbSet
                            .Where(x => x.Status == orderState)
                            .Include(x => x.User)
                            .Include(x => x.Products)
                            .ThenInclude(x => x.Product)
                            .ToListAsync();

        return _mapper.Map<List<OrderDTO>>(orders);
    }

    public async Task<OrderDTO> Get(string id)
    {
        var order = await _dbSet
                            .Include(x => x.User)
                            .Include(x => x.Products)
                            .ThenInclude(x => x.Product)
                            .FirstOrDefaultAsync();

        if (order == null)
        {
            throw new AppException(HttpStatusCode.NotFound, String.Format(ErrorMessages.NOT_FOUND_ERROR, "Order", "Id", id));
        }

        return _mapper.Map<OrderDTO>(order);
    }
}