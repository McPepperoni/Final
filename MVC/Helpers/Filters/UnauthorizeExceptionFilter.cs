using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Filters;

using MVC.DTOs;

namespace MVC.Helpers.Filters;

public class UnauthorizeExceptionFilter : ExceptionFilterAttribute
{
    private HttpClient _client { get; set; }

    public override async Task OnExceptionAsync(ExceptionContext context)
    {
        var scope = context.HttpContext.RequestServices.CreateScope();
        var services = scope.ServiceProvider;

        Console.WriteLine(12);

        _client = services.GetRequiredService<IHttpClientFactory>().CreateClient("ProductAPIClient");

        if (context.Exception is UnauthorizedAccessException)
        {
            var result = await _client.PostAsJsonAsync("Auth/RefreshToken", new
            {
                refreshToken = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "RefreshToken")
            });

            if (result.IsSuccessStatusCode)
            {
                var claimsPrincipal = context.HttpContext.User;
                await context.HttpContext.SignOutAsync("Cookies");

                var content = await result.Content.ReadFromJsonAsync<AuthTokenResultDTO>();

                var handler = new JwtSecurityTokenHandler();

                var token = handler.ReadJwtToken(content.AccessToken);

                var claims = ((JwtSecurityToken)token).Claims.ToList();

                claims.Add(new Claim("AccessToken", content.AccessToken));
                claims.Add(new Claim("RefreshToken", content.RefreshToken));

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.FromUnixTimeSeconds(int.Parse(claims.Where(x => x.Type == JwtRegisteredClaimNames.Exp).FirstOrDefault().Value))
                };

                await context.HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                await context.HttpContext.ChallengeAsync("Bearer");
            }
        }
    }
}