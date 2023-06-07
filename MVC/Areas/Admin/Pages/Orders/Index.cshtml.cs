using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.DTOs;
using Persistence.Entities;

namespace MVC.Areas.Admin.Pages.Orders;

[Authorize(Roles = "Admin")]
public class IndexModel : BaseAuthorizedPageModel
{
    public List<OrderDTO> Orders { get; set; }
    public string Status { get; set; }
    public class InputModel
    {

    }
    public IndexModel(IHttpClientFactory factory, IHttpContextAccessor accessor) : base(factory, accessor)
    {

    }

    public async Task OnGetAsync(string orderStatus = OrderStatus.WAIT_FOR_APPROVAL)
    {
        Status = orderStatus;
        var response = await _client.GetAsync($"Order/all/{orderStatus}");
        var content = await response.Content.ReadFromJsonAsync<List<OrderDTO>>();

        Orders = content;
    }

    public async Task<IActionResult> OnGetUpdateOrder(string id, string currentState)
    {
        var nextState = "";
        switch (currentState)
        {
            case OrderStatus.WAIT_FOR_APPROVAL:
                nextState = OrderStatus.DELIVERING;
                break;
            case OrderStatus.DELIVERING:
                nextState = OrderStatus.DELIVERED;
                break;
            default:
                nextState = OrderStatus.WAIT_FOR_APPROVAL;
                break;
        }

        var response = await _client.GetAsync($"Order/{id}/{nextState}");

        return Redirect($"/Admin/Orders?OrderStatus={nextState}");
    }

    public async Task<IActionResult> OnGetCancelOrder(string id)
    {
        var response = await _client.DeleteAsync($"Order/{id}");

        return Redirect($"/Admin/Orders?OrderStatus={OrderStatus.CANCELED}");
    }
}