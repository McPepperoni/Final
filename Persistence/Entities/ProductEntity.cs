namespace Persistence.Entities;

public class ProductEntity : BaseEntity, ISoftDeletable
{
    public string ImgSrc { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public int Quantity { get; set; }
    public bool PublicStatus { get; set; }
    public DateTime? DeletedAt { get; set; }
    public virtual List<ProductCategoryEntity> Categories { get; set; }
}