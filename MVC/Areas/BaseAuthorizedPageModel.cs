using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MVC.Areas;

[Authorize]
public class BaseAuthorizedPageModel : PageModel
{
    protected readonly HttpClient _client;

    public BaseAuthorizedPageModel(IHttpClientFactory factory, IHttpContextAccessor contextAccessor)
    {
        _client = factory.CreateClient("ProductAPIClient");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", contextAccessor.HttpContext.User.Claims.Where(x => x.Type == "JWT").FirstOrDefault().Value);
    }
}