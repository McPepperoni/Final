using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MVC.DTOs;

namespace MVC.Areas.Product;

public class CartModel : PageModel
{
    private readonly HttpClient _client;

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        public List<CartProductDTO> CartItems { get; set; }
        public int TotalPrice { get; set; }
        public List<bool> CheckedItems { get; set; }
    }

    public CartModel(IHttpClientFactory factory)
    {
        _client = factory.CreateClient("ProductAPIClient");
    }

    public string ReturnURL { get; set; }

    public async Task OnGetAsync(string returnURL)
    {
        ReturnURL = returnURL;

        var UserId = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault().Value;
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.Claims.Where(x => x.Type == "JWT").FirstOrDefault().Value);

        var response = await _client.GetAsync($"Cart/{UserId}");
        var cart = await response.Content.ReadFromJsonAsync<CartDTO>();
        Input = new InputModel
        {
            CartItems = cart.CartProducts,
            CheckedItems = new List<bool>(new bool[cart.CartProducts.Count]),
            TotalPrice = cart.TotalPrice,
        };
    }

    public async Task<IActionResult> OnGetRemoveFromCart(string itemId)
    {
        var UserId = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault().Value;
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.Claims.Where(x => x.Type == "JWT").FirstOrDefault().Value);

        var response = await _client.DeleteAsync($"CartItem/{itemId}");

        return RedirectToPage();
    }
}