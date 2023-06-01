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
    public bool PublicStatus { get; set; }
    public bool PublicStatusValue { get; set; } = true;
}

public class ProductCreateDTO
{
    public string ImgSrc { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public int Quantity { get; set; }
    public bool PublicStatus { get; set; }
    public List<string> CategoryIds { get; set; }
}
public class ProductUpdateDTO
{
    public string ImgSrc { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public int Quantity { get; set; }
    public bool PublicStatus { get; set; }
    public List<string> CategoryIds { get; set; }
}

public class ProductPaginationResponseDTO : PaginationResponseDTO<ProductDTO>
{
    public List<string> Categories { get; set; }

    public ProductPaginationResponseDTO(ProductPaginationRequestDTO paginationRequest, List<ProductDTO> data) : base(paginationRequest, data)
    {
        Categories = new List<string>();
        foreach (var item in data)
        {
            foreach (var category in item.Categories)
            {
                Categories.Add(category.Category.Name);
            }
        }

        Categories = Categories.Distinct().ToList();
    }
}