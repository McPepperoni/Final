namespace WebApi.DTOs;

public class ProductDTO : BaseDTO
{
    public string ImgSrc { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public int Quantity { get; set; }
    public bool PublicStatus { get; set; }
    public List<ProductCategoryDTO> Categories { get; set; }
}

public class ProductPaginationRequestDTO : PaginationRequestDTO
{
    public string SearchTerm { get; set; }
    public int PriceMin { get; set; } = int.MinValue;
    public int PriceMax { get; set; } = int.MaxValue;
    public string Categories { get; set; }
}