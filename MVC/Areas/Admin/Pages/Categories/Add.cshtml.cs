using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.DTOs;

namespace MVC.Areas.Admin.Pages.Categories;

[Authorize(Roles = "Admin")]
public class AddModel : BaseAuthorizedPageModel
{
    [BindProperty]
    public InputModel Input { get; set; }
    public class InputModel
    {
        [Required]
        public CreateCategoryDTO Category { get; set; }
    }
    public AddModel(IHttpClientFactory factory, IHttpContextAccessor accessor) : base(factory, accessor)
    {
    }
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync(string categoryIds)
    {

        if (ModelState.IsValid)
        {
            var response = await _client.PostAsJsonAsync($"Category", Input.Category);
            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                var error = await response.Content.ReadFromJsonAsync<ErrorDTO>();
                ModelState.AddModelError("Existed", error.Message);
                return Page();
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