namespace Persistence.Entities;

public class ProductCategoryEntity : BaseEntity
{
    public string ProductId { get; set; }
    public string CategoryId { get; set; }
    public virtual ProductEntity Product { get; set; }
    public virtual CategoryEntity Category { get; set; }
}