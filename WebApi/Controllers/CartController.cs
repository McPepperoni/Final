using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs.CartDTO;
using WebApi.Services;

namespace WebApi.Controllers;

public class CartController : BaseController
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartDetailDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var res = await _cartService.Get();
        return Ok(res);
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartDetailDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut]
    public async Task<IActionResult> Update(UpdateCartDTO updateCart)
    => Ok(await _cartService.Update(updateCart));

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartDetailDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{itemId}")]
    public async Task<IActionResult> RemoveFromCart(string itemId)
    => Ok(await _cartService.RemoveFromCart(itemId));

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartDetailDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartDTO addToCart)
    => Ok(await _cartService.AddToCart(addToCart));
}