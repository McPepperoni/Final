using System.ComponentModel.DataAnnotations;

namespace MVC.DTOs;

public class CartProductDTO : BaseDTO
{
    [Required]
    public int Quantity { get; set; }
    [Required]
    public ProductDTO Product { get; set; }
}