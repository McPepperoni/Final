using FluentValidation;
using WebApi.DTOs;

namespace WebApi.DTOs;

public class UserDTO : BaseDTO
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public UserInfoDTO UserInfo { get; set; }
}

public class UserDTOValidator : AbstractValidator<UserDTO>
{
    public UserDTOValidator()
    {
        RuleFor(x => x.UserName).UserName();
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
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
}

public class UserPatchDTOValidator : AbstractValidator<UserPatchDTO>
{
    public UserPatchDTOValidator()
    {
        RuleFor(x => x.UserName).UserName();
        RuleFor(x => x.Password).Password();
        RuleFor(x => x.Email).Email();
        RuleFor(x => x.PhoneNumber).PhoneNumber();
        RuleFor(x => x.Address).Address();
    }
}

public class UserSignUpDTO
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
}

public class UserSignUpDTOValidator : AbstractValidator<UserSignUpDTO>
{
    public UserSignUpDTOValidator()
    {
        RuleFor(x => x.UserName).UserName();
        RuleFor(x => x.Password).Password();
        RuleFor(x => x.Email).Email();
        RuleFor(x => x.FirstName).FirstName();
        RuleFor(x => x.LastName).LastName();
        RuleFor(x => x.PhoneNumber).PhoneNumber();
        RuleFor(x => x.Address).Address();
    }
}

public class VerifyUserCredentialDTO
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}

public class VerifyUserCredentialDTOValidator : AbstractValidator<VerifyUserCredentialDTO>
{
    public VerifyUserCredentialDTOValidator()
    {
        RuleFor(x => x.UserName).UserName();
        RuleFor(x => x.Email).Email();
        RuleFor(x => x.PhoneNumber).PhoneNumber();
    }
}

public class RefreshTokenDTO
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}

public class SearchRequestUserDTO
{
    public string Query { get; set; }
    public int Limit { get; set; } = 10;
    public bool IncludeEmail { get; set; } = true;
    public bool IncludeName { get; set; } = true;
}

public class SearchUserDTOValidator : AbstractValidator<SearchRequestUserDTO>
{
    public SearchUserDTOValidator()
    {
        RuleFor(x => x.Query).NotEmpty().MinimumLength(3).WithMessage("Query string must be longer than 3 characters");
    }
}

public class SearchResponseUserDTO
{
    public SearchRequestUserDTO SearchParams { get; set; }
    public UserSearchResultDTO Result { get; set; }
}

public class UserSearchResultDTO
{
    public SearchResultDTO<List<UserDTO>> ByEmail { get; set; }
    public SearchResultDTO<List<UserDTO>> ByName { get; set; }
}
