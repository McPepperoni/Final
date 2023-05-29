namespace Persistence.Entities;

public class ProductEntity : BaseEntity
{
    public string ImgSrc { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public int Quantity { get; set; }
    public bool PublicStatus { get; set; }
    public virtual List<ProductCategoryEntity> Categories { get; set; }
}