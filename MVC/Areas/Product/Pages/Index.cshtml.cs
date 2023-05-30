using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MVC.DTOs;

namespace MVC.Areas.Product;

public class Query
{
    public int CurrentPage { get; set; }
}

[Authorize]
public class IndexModel : PageModel
{
    private readonly HttpClient _client;

    public string Title { get; set; }
    public PaginationResponseDTO<ProductDTO, ProductPaginationRequestDTO> Data { get; set; }
    [FromQuery(Name = "Page")]
    public int CurrentPage { get; set; }
    public class InputModel
    {
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
    }
    public IndexModel(IHttpClientFactory factory)
    {
        _client = factory.CreateClient("ProductAPIClient");
    }

    public async Task OnGetAsync()
    {
        var response = await _client.GetAsync($"Products?Page={CurrentPage - 1}");
        Data = await response.Content.ReadFromJsonAsync<PaginationResponseDTO<ProductDTO, ProductPaginationRequestDTO>>();

    }

    public IActionResult OnGetNextPage()
    {
        CurrentPage++;

        return Page();
    }
}