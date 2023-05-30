using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MVC.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public IActionResult OnGet()
    {
        return User.Identity.IsAuthenticated ? Redirect("Product") : Redirect("Identity/Account/Login");
    }

    public async Task OnPostSignOut()
    {
        await HttpContext.SignOutAsync();
    }
}
