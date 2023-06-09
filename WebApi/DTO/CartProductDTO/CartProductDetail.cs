using WebApi.DTOs.ProductDTO;

namespace WebApi.DTOs.CartDTO;

public class CartProductDetailDTO : BaseDTO
{
    public int Quantity { get; set; }
    public ProductDetailDTO Product { get; set; }
}