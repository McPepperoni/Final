using FluentValidation;

namespace WebApi.DTOs;

public class UserInfoDTO : BaseDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
}

public class UserInfoDTOValidator : AbstractValidator<UserInfoDTO>
{
    public UserInfoDTOValidator()
    {
        RuleFor(x => x.FirstName).FirstName();
        RuleFor(x => x.LastName).LastName();
    }
}