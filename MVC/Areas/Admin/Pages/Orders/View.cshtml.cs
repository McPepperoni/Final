using MVC.DTOs;

namespace MVC.Areas.Admin.Pages.Orders;

public class ViewModel : BaseAuthorizedPageModel
{
    public OrderDTO Order { get; set; }
    public ViewModel(IHttpClientFactory factory, IHttpContextAccessor accessor) : base(factory, accessor)
    {

    }

    public async Task OnGetAsync(string id)
    {
        var response = await _client.GetAsync($"Order/{id}");

        Order = await response.Content.ReadFromJsonAsync<OrderDTO>();
    }
}