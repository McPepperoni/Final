using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs.OrderDTO;
using WebApi.Services;

namespace WebApi.Controllers;

public class OrderController : BaseController
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderDTO createOrder)
    {
        await _orderService.CreateAsync(createOrder);
        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}/{orderStatus}")]
    public async Task<IActionResult> Proceed(string id, string orderStatus)
    {
        await _orderService.UpdateOrder(id, orderStatus);

        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Cancel(string id)
    {
        await _orderService.CancelOrderAsync(id);

        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OrderDetailDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("all/{orderState}")]
    public async Task<IActionResult> Filter(string orderState)
    => Ok(await _orderService.Filter(orderState));

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OrderDetailDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    => Ok(await _orderService.Get(id));


}