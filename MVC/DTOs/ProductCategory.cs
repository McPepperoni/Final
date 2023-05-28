namespace MVC.DTOs;

public class ProductCategoryDTO : BaseDTO
{
    public ProductDTO Product { get; set; }
    public CategoryDTO Category { get; set; }
}
