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
    [HttpGet("{userId}")]
    public async Task<IActionResult> Get(string userId)
    {
        var res = await _cartService.Get(userId);
        return Ok(res);
    }

    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartDTO))]
    [HttpPost("{userId}")]
    public async Task Create(string userId)
    {
        await _cartService.CreateAsync(userId);
        NoContent();
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut]
    public async Task<IActionResult> Update(UpdateCartDTO updateCart)
    => Ok(await _cartService.Update(updateCart));


}