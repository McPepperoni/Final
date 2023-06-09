namespace WebApi.DTOs.ProductDTO;

public class ProductUpdateDTO
{
    public string ImgSrc { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public int Quantity { get; set; }
    public bool PublicStatus { get; set; }
    public List<string> CategoryIds { get; set; }
}