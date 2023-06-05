using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.DTOs;

namespace MVC.Areas.Admin.Pages.Users;

[Authorize(Roles = "Admin")]
public class AddModel : BaseAuthorizedPageModel
{
    [BindProperty]
    public InputModel Input { get; set; }
    public class InputModel
    {
        public CreateUserDTO User { get; set; }
    }
    public AddModel(IHttpClientFactory factory, IHttpContextAccessor accessor) : base(factory, accessor)
    {
    }
    public void OnGet()
    {

    }

    public async Task<IActionResult> OnPostAsync(string id)
    {
        if (ModelState.IsValid)
        {
            var response = await _client.PostAsJsonAsync($"User", Input.User);

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                var error = await response.Content.ReadFromJsonAsync<ErrorDTO>();
                ModelState.AddModelError("Existed", error.Message);
                return Page();
            }

            return Redirect($"/Admin/Users");
        }

        return Page();
    }
}