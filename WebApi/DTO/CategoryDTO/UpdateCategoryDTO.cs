using FluentValidation;

namespace WebApi.DTOs.CategoryDTO;

public class UpdateCategoryDTO
{
    public string Name { get; set; }
}

public class UpdateCategoryDTOValidator : AbstractValidator<UpdateCategoryDTO>
{
    public UpdateCategoryDTOValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}