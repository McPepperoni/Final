using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MVC.Constants;
using MVC.DTOs;
using Newtonsoft.Json;

namespace MVC.Areas.Admin.Pages.Categories;

[Authorize(Roles = "Admin")]
public class EditModel : BaseAuthorizedPageModel
{
    [BindProperty]
    public InputModel Input { get; set; }
    public CategoryDTO Category { get; set; }
    public class InputModel
    {
        public UpdateCategoryDTO Category { get; set; }
    }
    public EditModel(IHttpClientFactory factory, IHttpContextAccessor accessor) : base(factory, accessor)
    {
    }

    public async Task OnGetAsync(string id)
    {
        var response = await _client.GetAsync($"Category/{id}");
        Category = await response.Content.ReadFromJsonAsync<CategoryDTO>();
    }

    public async Task<IActionResult> OnPostAsync(string id)
    {

        if (ModelState.IsValid)
        {
            var response = await _client.PutAsJsonAsync($"Category/{id}", Input.Category);
            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                ModelState.AddModelError("Existed", String.Format(ErrorMessages.CONFLICTED_ERROR, "Category", "Name", Input.Category.Name));
                return RedirectToPage();
            }
            if (!response.IsSuccessStatusCode)
            {
                return Redirect("/Error");
            }
            return Redirect($"/Admin/Categories");
        }

        return RedirectToPage();
    }
}