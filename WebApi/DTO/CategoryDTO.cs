using FluentValidation;

namespace WebApi.DTOs;

public class CategoryDTO : BaseDTO
{
    public string Name { get; set; }
}

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