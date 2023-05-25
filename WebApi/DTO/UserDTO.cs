using FluentValidation;
using WebApi.DTOs;

namespace WebApi.DTOs;

public class UserDTO : BaseDTO
{
    public string Email { get; set; }
    public string Role { get; set; }
    public UserInfoDTO UserInfo { get; set; }
    public List<UserRoleDTO> UserRoles { get; set; }
}

public class UserDTOValidator : AbstractValidator<UserDTO>
{
    public UserDTOValidator()
    {
        RuleFor(x => x.Email).Email();
        RuleFor(x => x.UserInfo).SetValidator(new UserInfoDTOValidator());
    }
}



public class UserLoginDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class UserLoginDTOValidator : AbstractValidator<UserLoginDTO>
{
    public UserLoginDTOValidator()
    {
        RuleFor(x => x.Password).Password();
        RuleFor(x => x.Email).Email();
    }
}

public class GetUserByIdDTO
{
    public string Id { get; set; }
}

public class GetUserByIdDTOValidator : AbstractValidator<GetUserByIdDTO>
{
    public GetUserByIdDTOValidator()
    {
        RuleFor(x => x.Id).NotEmpty().Length(36);
    }
}

public class DeleteUserDTO
{
    public string Id { get; set; }
}

public class DeleteUserDTOValidator : AbstractValidator<DeleteUserDTO>
{
    public DeleteUserDTOValidator()
    {
        RuleFor(x => x.Id).NotEmpty().Length(36);
    }
}

public class UserPatchDTO
{
    public string Password { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
}

public class UserPatchDTOValidator : AbstractValidator<UserPatchDTO>
{
    public UserPatchDTOValidator()
    {
        RuleFor(x => x.Password).Password();
        RuleFor(x => x.Email).Email();
        RuleFor(x => x.PhoneNumber).PhoneNumber();
        RuleFor(x => x.Address).Address();
    }
}

public class UserSignUpDTO
{
    public string Password { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
}

public class UserSignUpDTOValidator : AbstractValidator<UserSignUpDTO>
{
    public UserSignUpDTOValidator()
    {
        RuleFor(x => x.Password).Password();
        RuleFor(x => x.Email).Email();
        RuleFor(x => x.Name).Name();
        RuleFor(x => x.PhoneNumber).PhoneNumber();
        RuleFor(x => x.Address).Address();
    }
}

public class VerifyUserCredentialDTO
{
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}

public class VerifyUserCredentialDTOValidator : AbstractValidator<VerifyUserCredentialDTO>
{
    public VerifyUserCredentialDTOValidator()
    {
        RuleFor(x => x.Email).Email();
        RuleFor(x => x.PhoneNumber).PhoneNumber();
    }
}

public class RefreshTokenDTO
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}