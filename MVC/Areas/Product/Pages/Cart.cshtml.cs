using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MVC.DTOs;

namespace MVC.Areas.Product;

[Authorize(Roles = "User")]
public class CartModel : BaseAuthorizedPageModel
{

    [BindProperty]
    public InputModel Input { get; set; }
    public string Error { get; set; }

    public class InputModel
    {
        public List<CartProductDTO> CartItems { get; set; }
        public int TotalPrice { get; set; }
        public List<CartItem> CheckedItems { get; set; }

        public class CartItem
        {
            public bool Checked { get; set; }

            [RegularExpression(@"^\d*$")]
            public string Quantity { get; set; }
        }
    }

    public CartModel(IHttpClientFactory factory, IHttpContextAccessor contextAccessor) : base(factory, contextAccessor)
    {
    }

    public string ReturnURL { get; set; }

    public async Task OnGetAsync(string returnURL)
    {
        ReturnURL = returnURL == null ? "/Product" : returnURL;

        var response = await _client.GetAsync($"Cart");
        var cart = await response.Content.ReadFromJsonAsync<CartDTO>();
        var checkedItems = new List<InputModel.CartItem>();
        foreach (var item in cart.CartProducts)
        {
            checkedItems.Add(new InputModel.CartItem
            {
                Checked = true,
                Quantity = item.Quantity.ToString()
            });
        }
        Input = new InputModel
        {
            CartItems = cart.CartProducts,
            CheckedItems = checkedItems,
            TotalPrice = cart.TotalPrice,
        };
    }

    public async Task<IActionResult> OnGetRemoveFromCart(string itemId)
    {
        var response = await _client.DeleteAsync($"Cart/{itemId}");

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostPlaceOrder(string itemIds, string err = null)
    {
        Error = err;
        var ids = itemIds.Split(',');
        var requestBody = new CreateOrderDTO()
        {
            Products = new List<CreateOrderProductDTO>()
        };

        foreach (var (item, i) in ids.Select((x, i) => (x, i)))
        {
            if (Input.CheckedItems[i].Checked)
            {
                requestBody.Products.Add(new CreateOrderProductDTO
                {
                    ProductId = item,
                    Quantity = int.Parse(Input.CheckedItems[i].Quantity)
                });
            }
        }

        var response = await _client.PostAsJsonAsync("Order", requestBody);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            var content = await response.Content.ReadFromJsonAsync<ErrorDTO>();
            Redirect($"/Product/Cart?={content.Message}");
        }

        return RedirectToPage();
    }
}