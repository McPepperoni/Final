using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MVC.DTOs;

namespace MVC.Areas.Admin.Pages.Products;

public class IndexModel : BaseAuthorizedPageModel
{
    private readonly IMapper _mapper;
    public InputModel Input { get; set; }
    public class InputModel
    {
        public List<DisplayListDTO> Products { get; set; }
    }

    public IndexModel(IMapper mapper, IHttpContextAccessor accessor, IHttpClientFactory factory) : base(factory, accessor)
    {
        _mapper = mapper;
    }

    public async Task OnGet()
    {
        var response = await _client.GetAsync("Products");

        if (!response.IsSuccessStatusCode)
        {
            Redirect("/Forbidden/Error");
        }
        var products = await response.Content.ReadFromJsonAsync<ProductPaginationResponseDTO>();

        Input = new InputModel()
        {
            Products = _mapper.Map<List<DisplayListDTO>>(products.Data),
        };
    }

    public async Task<IActionResult> OnGetDelete(string id)
    {
        var response = await _client.DeleteAsync($"Products/{id}");

        return Redirect("/Admin/Products");
    }
}