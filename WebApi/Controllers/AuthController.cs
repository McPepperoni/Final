using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs.AuthDTO;
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultAuthDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginAuthDTO authLogin)
        => Ok(await _authService.Login(authLogin));

    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("Logout")]
    public async Task<IActionResult> Logout(RefreshTokenAuth token)
    {
        await _authService.Logout(token);
        return NoContent();
    }

    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultAuthDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenAuth token)
        => Ok(await _authService.RefreshToken(token));
}