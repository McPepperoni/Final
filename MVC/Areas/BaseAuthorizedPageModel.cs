using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

using MVC.Helpers.Filters;

namespace MVC.Areas;

[Authorize]
[UnauthorizeExceptionFilter]
public class BaseAuthorizedPageModel : PageModel
{
    protected readonly HttpClient _client;

    public BaseAuthorizedPageModel(IHttpClientFactory factory, IHttpContextAccessor contextAccessor)
    {
        _client = factory.CreateClient("ProductAPIClient");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "AccessToken").Value);
    }
}