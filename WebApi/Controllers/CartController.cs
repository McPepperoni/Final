using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.Services;

namespace WebApi.Controllers;

public class CartController : BaseController
{
    private readonly CartService _cartService;

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] UserCartDTO cart)
    => Ok(await _cartService.Get(cart.UserId));

    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartDTO))]
    [HttpPost]
    public async Task<IActionResult> Create([FromQuery] UserCartDTO cart)
    => Ok(await _cartService.Get(cart.UserId));
}