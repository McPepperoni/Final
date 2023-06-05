using System.ComponentModel.DataAnnotations;

namespace MVC.DTOs;

public class CartDTO : BaseDTO
{
    public string UserId { get; set; }
    public int TotalPrice { get; set; }
    public List<CartProductDTO> CartProducts { get; set; }
}

public class AddToCartDTO
{
    [Required]
    public string ItemId { get; set; }
    [Required]
    public int Quantity { get; set; }
}

public class RemoveFromCartDTO
{
    [Required]
    public string Id { get; set; }
    [Required]
    public string ItemId { get; set; }
}