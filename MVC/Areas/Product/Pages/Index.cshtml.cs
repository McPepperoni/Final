using System.Collections.Specialized;
using System.Net.Http.Headers;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.JsonWebTokens;
using MVC.DTOs;

namespace MVC.Areas.Product;

[Authorize(Roles = "User")]
public class IndexModel : PageModel
{
    private readonly HttpClient _client;
    public ProductPaginationResponseDTO Data { get; set; }

    public string Title { get; set; }
    [FromQuery(Name = "Page")]
    public int CurrentPage { get; set; }
    [FromQuery(Name = "Query")]
    public string Query { get; set; }
    [FromQuery(Name = "Categories")]
    public string Categories { get; set; }
    [FromQuery(Name = "PriceMin")]
    public int PriceMin { get; set; } = 0;
    [FromQuery(Name = "PriceMax")]
    public int PriceMax { get; set; } = 0;

    [BindProperty]
    public InputModel Input { get; set; }
    [BindProperty]
    public List<ProductDTO> Products { get; set; }
    public class InputModel
    {
        public string SearchQuery { get; set; }
        public Filter ProductFilter { get; set; }

        public class Filter
        {
            public int PriceMin { get; set; }
            public int PriceMax { get; set; }
            [BindProperty]
            public List<CategoryFilter> Categories { get; set; }
        }

        public class CategoryFilter
        {
            [BindProperty]
            public bool IsChecked { get; set; }
            [BindProperty]
            public string Title { get; set; }
        }
    }

    public IndexModel(IHttpClientFactory factory)
    {
        _client = factory.CreateClient("ProductAPIClient");
    }

    public async Task OnGetAsync()
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.Claims.Where(x => x.Type == "JWT").FirstOrDefault().Value);
        var queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
        queryString.Add("SearchTerm", Query);
        queryString.Add("Page", CurrentPage.ToString());
        queryString.Add("Categories", Categories);

        if (!(PriceMin == 0 && PriceMax == 0))
        {
            queryString["PriceMin"] = MathF.Min(PriceMin, PriceMax).ToString();
            queryString["PriceMax"] = MathF.Max(PriceMin, PriceMax).ToString();
        }

        var response = await _client.GetAsync($"Products?{queryString.ToString()}");
        if (!response.IsSuccessStatusCode)
        {
            Redirect("/Forbidden/Error");
        }

        Data = await response.Content.ReadFromJsonAsync<ProductPaginationResponseDTO>();
        var selectedCategories = new List<string>();
        if (Categories != null)
        {
            selectedCategories = Categories.Split(",").ToList();
        }

        var categoryFilter = Data.Categories.Select(x => new InputModel.CategoryFilter { Title = x, IsChecked = selectedCategories.Contains(x) }).ToList();
        var dataFilter = new InputModel.Filter
        {
            Categories = categoryFilter,
            PriceMin = PriceMin,
            PriceMax = PriceMax,
        };

        Input = new InputModel
        {
            SearchQuery = Query,
            ProductFilter = dataFilter,
        };

        Products = Data.Data;
    }

    public IActionResult OnPostSearch()
    {
        var queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
        queryString.Add("Query", Input.SearchQuery);

        return Redirect($"/Product?{queryString.ToString()}");
    }

    public IActionResult OnPostFilter(string category, string currentURI)
    {
        var categories = category.Split(",").ToList();
        var indexes = Input.ProductFilter.Categories.Where(x => x.IsChecked).Select((_, i) => i).ToList();

        var categoryFilter = categories
                                .Select((x, i) => (x, i))
                                .Where((x, i) => indexes.Contains(i))
                                .Select(x => x.x)
                                .ToList();

        NameValueCollection param;
        if (currentURI == null)
        {
            param = HttpUtility.ParseQueryString(String.Empty);
        }
        else
        {
            param = HttpUtility.ParseQueryString(currentURI);
        }

        param["Categories"] = $"{String.Join(",", categoryFilter)}";
        if (!(Input.ProductFilter.PriceMin == 0 && Input.ProductFilter.PriceMax == 0))
        {
            param["PriceMin"] = MathF.Min(Input.ProductFilter.PriceMin, Input.ProductFilter.PriceMax).ToString();
            param["PriceMax"] = MathF.Max(Input.ProductFilter.PriceMin, Input.ProductFilter.PriceMax).ToString();
        }

        return Redirect($"/Product?{param.ToString()}");
    }

    public async Task<IActionResult> OnPostAddToCartAsync(string id)
    {
        var userId = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Sub);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.Claims.Where(x => x.Type == "JWT").FirstOrDefault().Value);

        var response = await _client.GetAsync($"Cart?UserId={userId}");

        var content = response.Content.ReadFromJsonAsync<CartDTO>();

        return RedirectToPage();
    }
}