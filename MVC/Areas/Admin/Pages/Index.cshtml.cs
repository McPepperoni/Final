using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MVC.Areas.Admin;

[Authorize(Roles = "Admin")]
public class IndexModel : BaseAuthorizedPageModel
{
    public IndexModel(IHttpClientFactory factory, IHttpContextAccessor accessor) : base(factory, accessor)
    {

    }
}