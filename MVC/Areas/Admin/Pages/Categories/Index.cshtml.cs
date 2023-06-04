using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MVC.DTOs;

namespace MVC.Areas.Admin.Pages.Categories;

public class IndexModel : BaseAuthorizedPageModel
{

    private readonly IMapper _mapper;
    public InputModel Input { get; set; }
    public class InputModel
    {
        public List<DisplayListDTO> Categories { get; set; }
    }

    public IndexModel(IHttpClientFactory factory, IHttpContextAccessor accessor, IMapper mapper) : base(factory, accessor)
    {
        _mapper = mapper;
    }

    public async Task OnGetAsync()
    {
        var response = await _client.GetAsync("Category");

        var categories = await response.Content.ReadFromJsonAsync<List<CategoryDTO>>();
        Input = new InputModel()
        {
            Categories = _mapper.Map<List<DisplayListDTO>>(categories),
        };
    }

    public async Task<IActionResult> OnGetDelete(string id)
    {
        var response = await _client.DeleteAsync($"Category/{id}");

        return Redirect("/Admin/Categories");
    }
}