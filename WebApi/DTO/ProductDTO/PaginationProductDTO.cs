namespace WebApi.DTOs.ProductDTO;

public class ProductPaginationResponseDTO : PaginationResponseDTO<ProductDetailDTO>
{
    public List<string> Categories { get; set; }

    public ProductPaginationResponseDTO(ProductPaginationRequestDTO paginationRequest, List<ProductDetailDTO> data) : base(paginationRequest, data)
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