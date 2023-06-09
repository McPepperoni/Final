namespace WebApi.DTOs.CartDTO;

public class CartDetailDTO : BaseDTO
{
    public string UserId { get; set; }
    public int TotalPrice { get; set; }
    public List<CartProductDetailDTO> CartProducts { get; set; }
}