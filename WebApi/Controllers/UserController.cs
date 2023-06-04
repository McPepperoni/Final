using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.Services;

namespace WebApi.Controllers;

public class UserController : BaseController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<IActionResult> Register(CreateUserDTO createUser)
    {
        await _userService.Create(createUser);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserDTO>))]
    [HttpGet]
    public async Task<IActionResult> Get()
    => Ok(await _userService.Get());

    [HttpGet("{id:length(36)}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(string id)
    => Ok(await _userService.Get(id));

    [HttpPut("{id:length(36)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(string id, UpdateUserDTO updateUser)
    {
        await _userService.Update(id, updateUser);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:length(36)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(UserDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id)
    {
        await _userService.Delete(id);
        return NoContent();
    }
}