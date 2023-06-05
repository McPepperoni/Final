using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Filters;

using MVC.DTOs;

namespace MVC.Filters;

public class UnauthorizeExceptionFilter : ExceptionFilterAttribute
{
    private readonly HttpClient _client;

    public UnauthorizeExceptionFilter(IHttpClientFactory factory) : base()
    {
        _client = factory.CreateClient("ProductAPIClient");
    }

    public override async Task OnExceptionAsync(ExceptionContext context)
    {
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

                var tokens = await result.Content.ReadFromJsonAsync<AuthTokenResultDTO>();


            }
        }
    }
}