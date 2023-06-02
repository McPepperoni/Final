using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.Services;

namespace WebApi.Controllers;

public class CartItemController : BaseController
{
    private readonly ICartItemService _cartItemService;

    public CartItemController(ICartItemService cartItemService)
    {
        _cartItemService = cartItemService;
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveFromCart(string id)
    => Ok(await _cartItemService.RemoveFromCart(id));

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost("{userId}")]
    public async Task<IActionResult> AddToCart([FromQuery] string userId, [FromBody] AddToCartDTO addToCart)
    => Ok(await _cartItemService.AddToCart(userId, addToCart));
}