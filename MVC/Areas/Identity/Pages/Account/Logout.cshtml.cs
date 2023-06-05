using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Areas.Identity.Pages.Account;

public class LogoutModel : BasePageModel
{
    public LogoutModel(IHttpClientFactory factory, IHttpContextAccessor accessor) : base(factory, accessor)
    {
    }

    public async Task<IActionResult> OnGetAsync()
    {
        await HttpContext.SignOutAsync("Cookies");

        return Redirect("/");
    }
}

