using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MVC.Areas;

public class BasePageModel : PageModel
{
    protected readonly HttpClient _client;

    public BasePageModel(IHttpClientFactory factory, HttpContextAccessor contextAccessor)
    {
        _client = factory.CreateClient("ProductAPIClient");
    }
}