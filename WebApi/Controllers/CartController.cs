using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.Services;

namespace WebApi.Controllers;

public class CartController : BaseController
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] UserCartDTO cart)
    {
        var res = await _cartService.Get(cart.UserId);
        return Ok(res);
    }

    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartDTO))]
    [HttpPost]
    public async Task Create([FromQuery] UserCartDTO cart)
    {
        await _cartService.Create(cart.UserId);
        NoContent();
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut]
    public async Task<IActionResult> Update(UpdateCartDTO updateCart)
    => Ok(await _cartService.Update(updateCart));
}