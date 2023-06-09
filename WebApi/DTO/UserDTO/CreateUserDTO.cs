using FluentValidation;

namespace WebApi.DTOs.UserDTO;

public class CreateUserDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
}

public class CreateUserDTOValidator : AbstractValidator<CreateUserDTO>
{
    public CreateUserDTOValidator()
    {
        RuleFor(x => x.Email).Email();
        RuleFor(x => x.Password).Password();
        RuleFor(x => x.FullName).Name();
        RuleFor(x => x.Address).Address();
        RuleFor(x => x.PhoneNumber).PhoneNumber();
    }
}