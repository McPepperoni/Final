namespace MVC.DTOs;

public class CartDTO : BaseDTO
{
    public string UserId { get; set; }
    public List<CartProductDTO> CartProducts { get; set; }
}