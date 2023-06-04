using System.ComponentModel.DataAnnotations;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.DTOs;

namespace MVC.Areas.Admin.Pages.Products;

[Authorize(Roles = "Admin")]
public class AddModel : BaseAuthorizedPageModel
{
    private readonly IMapper _mapper;
    [BindProperty]
    public InputModel Input { get; set; }
    public class InputModel
    {
        public class Category
        {
            public string Name { get; set; }
            public string Id { get; set; }
            public bool Checked { get; set; }
        }

        public List<Category> Categories { get; set; }
        [Required]
        public CreateProductDTO Product { get; set; }
    }
    public AddModel(IMapper mapper, IHttpClientFactory factory, IHttpContextAccessor accessor) : base(factory, accessor)
    {
        _mapper = mapper;
    }
    public async Task OnGetAsync()
    {
        Input = new InputModel();

        var response = await _client.GetAsync("Category");
        var categories = await response.Content.ReadFromJsonAsync<List<CategoryDTO>>();

        Input.Categories = _mapper.Map<List<InputModel.Category>>(categories);
    }

    public async Task<IActionResult> OnPostAsync(string categoryIds)
    {
        if (ModelState.IsValid)
        {
            var categories = categoryIds.Split(",").Where((x, i) => Input.Categories[i].Checked);
            Input.Product.ImgSrc = "https://source.unsplash.com/random/800x800/?img=2";
            Input.Product.CategoryIds = categories.ToList();

            var response = await _client.PostAsJsonAsync($"Products", Input.Product);
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