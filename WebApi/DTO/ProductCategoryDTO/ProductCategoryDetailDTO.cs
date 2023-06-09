using WebApi.DTOs.CategoryDTO;
using WebApi.DTOs.ProductDTO;

namespace WebApi.DTOs.ProductCategoryDTO;

public class ProductCategoryDetailDTO : BaseDTO
{
    public ProductDetailDTO Product { get; set; }
    public CategoryDetailDTO Category { get; set; }
}