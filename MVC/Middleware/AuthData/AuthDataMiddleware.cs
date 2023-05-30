using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MVC.Middleware.AuthData;

public class AuthDataMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthDataMiddleware> _logger;

    public AuthDataMiddleware(RequestDelegate next, ILogger<AuthDataMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var tokenBytes = context.Session.Get("Auth-token");
        if (tokenBytes != null)
        {
            var authToken = Encoding.UTF8.GetString(tokenBytes);
            var handler = new JwtSecurityTokenHandler();

            var token = (await handler.ValidateTokenAsync(authToken, new())).SecurityToken;

            // ((JwtSecurityToken)token)
        }

        await _next(context);
    }
}