using WebApi.DTOs.ProductDTO;

namespace WebApi.DTOs.OrderProductDTO;

public class OrderProductDetailDTO : BaseDTO
{
    public int Quantity { get; set; }
    public ProductDetailDTO Product { get; set; }
}