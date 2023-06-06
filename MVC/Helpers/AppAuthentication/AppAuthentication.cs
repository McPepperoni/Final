using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using MVC.DTOs;

namespace Microsoft.Extensions.DependencyInjection;

public static class AppAuthentication
{
    public static IServiceCollection AddAppAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Forbidden";
        options.LoginPath = "/Identity/Account/Login";
        options.LogoutPath = "/Identity/Account/Logout";
        options.Events = new CookieAuthenticationEvents
        {
            OnValidatePrincipal = async context =>
            {
                if (context.HttpContext.User.Identity.IsAuthenticated)
                {
                    var exp = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value;
                    if (exp != null)
                    {
                        var expires = DateTime.Parse(exp, CultureInfo.InvariantCulture).ToUniversalTime();
                        string refreshToken;
                        if (expires < DateTime.UtcNow)
                        {
                            context.RejectPrincipal();
                            return;
                        }

                        refreshToken = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "RefreshToken").Value;

                        var scope = services.BuildServiceProvider().CreateScope();
                        var client = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("ProductAPIClient");

                        var response = await client.PostAsJsonAsync("Auth/RefreshToken", new
                        {
                            refreshToken = refreshToken,
                        });

                        if (!response.IsSuccessStatusCode)
                        {
                            context.RejectPrincipal();
                            return;
                        }

                        await context.HttpContext.SignOutAsync("Cookies");

                        var tokens = await response.Content.ReadFromJsonAsync<AuthTokenResultDTO>();
                        var handler = new JwtSecurityTokenHandler();

                        var token = handler.ReadJwtToken(tokens.AccessToken);

                        var claims = ((JwtSecurityToken)token).Claims.ToList();

                        claims.Add(new Claim("AccessToken", tokens.AccessToken));
                        claims.Add(new Claim("RefreshToken", tokens.RefreshToken));

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
                    }

                }
            }
        };
    });

        return services;
    }
}