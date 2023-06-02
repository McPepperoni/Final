namespace MVC.DTOs;

public class CartDTO : BaseDTO
{
    public string UserId { get; set; }
    public int TotalPrice { get; set; }
    public List<CartProductDTO> CartProducts { get; set; }
}

public class AddToCartDTO
{
    public string ItemId { get; set; }
    public int Quantity { get; set; }
}

public class RemoveFromCartDTO
{
    public string Id { get; set; }
    public string ItemId { get; set; }
}