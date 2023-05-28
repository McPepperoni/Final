using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.Helpers.JWT;
using WebApi.Services;

namespace WebApi.Controllers;


public class AuthController : BaseController
{
    private readonly IAuthService _authService;
    private readonly IJWTService _jwtService;
    private readonly JWTHelper _jwtHelper;

    public AuthController(IAuthService authService, IJWTService jWTService, JWTHelper jwtHelper)
    {
        _authService = authService;
        _jwtService = jWTService;
        _jwtHelper = jwtHelper;
    }

    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseRequestTokenDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost("Token")]
    public async Task<IActionResult> Token(UserDTO user)
    => Ok(await _authService.RequestToken(user));
}