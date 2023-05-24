namespace WebApi.Entities;

public class ProductCategoryEntity : BaseEntity
{
    public virtual ProductEntity Product { get; set; }
    public virtual CategoryEntity Category { get; set; }
}