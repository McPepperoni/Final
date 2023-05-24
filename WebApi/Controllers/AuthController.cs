using AutoMapper;
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

    /// <summary>
    /// To login
    /// </summary>
    /// <param name="userLogin"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponseDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Login([FromBody] UserLoginDTO userLogin)
        => Ok(await _authService.SignIn(userLogin));

    /// <summary>
    /// To signup
    /// </summary>
    /// <param name="userSignUp"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("signup")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> signUp([FromBody] UserSignUpDTO userSignUp)
        => Ok(await _authService.SignUp(userSignUp));

    /// <summary>
    /// To get new access token
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("refreshToken")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponseDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO refreshToken)
        => Ok(await _authService.RefreshToken(refreshToken));

    /// <summary>
    /// To logout
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout([FromBody] string refreshToken)
    {
        await _jwtService.Add(refreshToken);
        return NoContent();
    }
}