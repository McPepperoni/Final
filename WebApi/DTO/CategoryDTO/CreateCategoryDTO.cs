using FluentValidation;

namespace WebApi.DTOs.CategoryDTO;

public class CreateCategoryDTO
{
    public string Name { get; set; }
}

public class CreateCategoryDTOValidator : AbstractValidator<CreateCategoryDTO>
{
    public CreateCategoryDTOValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}