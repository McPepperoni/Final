namespace Persistence.Entities;

public class CategoryEntity : BaseEntity, ISoftDeletable
{
    public string Name { get; set; }
    public DateTime? DeletedAt { get; set; }
    public virtual List<ProductCategoryEntity> ProductCategories { get; set; }
}