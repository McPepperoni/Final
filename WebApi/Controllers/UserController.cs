using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.Helpers.JWT;
using WebApi.Services;

namespace WebApi.Controllers;


public class UserController : BaseController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService, JWTHelper jwtHelper)
    {
        _userService = userService;
    }

    /// <summary>
    /// To Get all Users
    /// </summary>
    /// <param name="paginationRequest"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponseDTO<UserDTO>))]
    public async Task<IActionResult> GetAll([FromQuery] PaginationRequestDTO paginationRequest)
        => Ok(await _userService.Get(paginationRequest));

    /// <summary>
    /// To patch an existing user
    /// </summary>
    /// <param name="userPatchDTO"></param>
    /// <returns></returns>
    [HttpPut("{id:length(36)}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch([FromBody] UserPatchDTO userPatchDTO, [FromRoute] string id)
    => Ok(await _userService.Update(userPatchDTO, id));

    /// <summary>
    /// To delete user with an Id
    /// </summary>
    /// <param name="id"></param>
    /// <remarks><strong>Require logged in</strong></remarks>
    /// <returns></returns>
    [HttpDelete("{id:length(36)}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public async Task<IActionResult> Delete(string id)
        => Ok(await _userService.Delete(id));
}