using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MVC.DTOs;

namespace MVC.Areas.Admin.Pages.Users;

[Authorize(Roles = "Admin")]
public class IndexModel : BaseAuthorizedPageModel
{
    private readonly IMapper _mapper;
    public InputModel Input { get; set; }
    public class InputModel
    {
        public List<DisplayListDTO> Users { get; set; }
    }
    public IndexModel(IHttpClientFactory factory, IHttpContextAccessor accessor, IMapper mapper) : base(factory, accessor)
    {
        _mapper = mapper;
    }

    public async Task OnGetAsync()
    {
        var response = await _client.GetAsync("User");
        var users = await response.Content.ReadFromJsonAsync<List<UserDTO>>();

        Input = new InputModel()
        {
            Users = _mapper.Map<List<DisplayListDTO>>(users),
        };
    }

    public async Task<IActionResult> OnGetDelete(string id)
    {
        var response = await _client.DeleteAsync($"User/{id}");

        return Redirect("/Admin/Users");
    }
}