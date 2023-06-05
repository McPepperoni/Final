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
    public string Name { get; set; }
}

public class UpdateCategoryDTO
{
    [Required]
    public string Name { get; set; }
}