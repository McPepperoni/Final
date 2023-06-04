using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC.DTOs;

public class CategoryDTO : BaseDTO
{
    public string Name { get; set; }
}

public class CreateCategoryDTO
{
    [Required]
    [DisplayName("Name")]
    public string Name { get; set; }
}

public class UpdateCategoryDTO
{
    public string Name { get; set; }
}