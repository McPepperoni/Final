using FluentValidation;

namespace WebApi.DTOs;

public class UserDTO
{
    public string Id { get; set; }
    public List<string> Roles { get; set; }
}

public class UserDTOValidator : AbstractValidator<UserDTO>
{
    public UserDTOValidator()
    {
        RuleFor(x => x.Id).Length(36).WithMessage("Invalid Id");
    }
}