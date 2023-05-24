namespace WebApi.Entities;

public class CategoryEntity : BaseEntity
{
    public string Name { get; set; }
    public virtual List<ProductCategoryEntity> ProductCategories { get; set; }
}