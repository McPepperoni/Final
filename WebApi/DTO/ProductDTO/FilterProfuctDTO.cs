namespace WebApi.DTOs.ProductDTO;

public class ProductPaginationRequestDTO : PaginationRequestDTO
{
    public string SearchTerm { get; set; }
    public int PriceMin { get; set; } = int.MinValue;
    public int PriceMax { get; set; } = int.MaxValue;
    public string Categories { get; set; }
    public bool PublicStatus { get; set; }
    public bool PublicStatusValue { get; set; } = true;
}