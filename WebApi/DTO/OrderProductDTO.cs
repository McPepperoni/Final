namespace WebApi.DTOs;

public class OrderProductDTO : BaseDTO
{
    public int Quantity { get; set; }
    public ProductDTO Product { get; set; }
}

public class CreateOrderProductDTO
{
    public int Quantity { get; set; }
    public string ProductId { get; set; }
}