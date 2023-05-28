namespace WebApi.DTOs;

public class CartProductDTO : BaseDTO
{
    public int Quantity { get; set; }
    public ProductDTO Product { get; set; }
}