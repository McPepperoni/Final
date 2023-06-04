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
        return User.Identity.IsAuthenticated ?
                User.IsInRole("User") ?
                Redirect("Product") :
                Redirect("Admin") :
                Redirect("Identity/Account/Login");
    }

}
