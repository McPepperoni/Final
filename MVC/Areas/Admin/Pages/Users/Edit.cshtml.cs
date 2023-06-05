using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.DTOs;

namespace MVC.Areas.Admin.Pages.Users;

[Authorize(Roles = "Admin")]
public class EditModel : BaseAuthorizedPageModel
{
    [BindProperty]
    public InputModel Input { get; set; }
    public UserDTO UserData { get; set; }
    public class InputModel
    {
        public UpdateUserDTO User { get; set; }
    }
    public EditModel(IHttpClientFactory factory, IHttpContextAccessor accessor) : base(factory, accessor)
    {
    }
    public async Task OnGetAsync(string id)
    {
        var response = await _client.GetAsync($"User/{id}");
        UserData = await response.Content.ReadFromJsonAsync<UserDTO>();
    }

    public async Task<IActionResult> OnPostAsync(string id)
    {
        if (ModelState.IsValid)
        {
            var response = await _client.PutAsJsonAsync($"User/{id}", Input.User);
            return Redirect($"/Admin/Users");
        }

        return Page();
    }
}