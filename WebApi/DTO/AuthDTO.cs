using FluentValidation;

namespace WebApi.DTOs;

public class AuthDTO
{

}

public class ResponseRequestTokenDTO
{
    public string AccessToken { get; set; }
    public UserDTO User { get; set; }
}

public class AuthRegisterDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public string Address { get; set; }
}

public class AuthRegisterDTOValidator : AbstractValidator<AuthRegisterDTO>
{
    public AuthRegisterDTOValidator()
    {
        RuleFor(x => x.Email).Email();
        RuleFor(x => x.Password).Password();
        RuleFor(x => x.FullName).NotEmpty();
        RuleFor(x => x.Address).NotEmpty();
    }
}

public class AuthLoginDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class AuthLoginDTOValidator : AbstractValidator<AuthLoginDTO>
{
    public AuthLoginDTOValidator()
    {
        RuleFor(x => x.Email).Email();
        RuleFor(x => x.Password).Password();
    }
}