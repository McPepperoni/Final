using System.ComponentModel.DataAnnotations;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.DTOs;

namespace MVC.Areas.Admin.Pages.Products;

[Authorize(Roles = "Admin")]
public class EditModel : BaseAuthorizedPageModel
{
    private readonly IMapper _mapper;
    [BindProperty]
    public InputModel Input { get; set; }
    public ProductDTO Product { get; set; }
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
        public ProductUpdateDTO Product { get; set; }
    }
    public EditModel(IMapper mapper, IHttpClientFactory factory, IHttpContextAccessor accessor) : base(factory, accessor)
    {
        _mapper = mapper;
    }
    public async Task OnGetAsync(string id)
    {
        Input = new InputModel();
        var response = await _client.GetAsync($"Products/{id}");
        var product = await response.Content.ReadFromJsonAsync<ProductDTO>();
        var selectedCategories = product.Categories.Select(x => x.Category.Id);

        response = await _client.GetAsync("Category");
        var allCategories = await response.Content.ReadFromJsonAsync<List<CategoryDTO>>();
        var categories = new List<InputModel.Category>();
        foreach (var item in allCategories)
        {
            categories.Add(new InputModel.Category
            {
                Id = item.Id,
                Name = item.Name,
                Checked = selectedCategories.Contains(item.Id),
            });
        }

        Input.Categories = categories;
        Product = product;
    }

    public async Task<IActionResult> OnPostAsync(string id, string categoryIds)
    {
        if (ModelState.IsValid)
        {
            var categories = categoryIds.Split(",").Where((x, i) => Input.Categories[i].Checked).ToList();
            Input.Product.ImgSrc = "https://source.unsplash.com/random/800x800/?img=2";
            Input.Product.CategoryIds = categories;

            var response = await _client.PutAsJsonAsync($"Products/{id}", Input.Product);
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
            return Redirect($"/Admin/Products");
        }

        return RedirectToPage();
    }
}