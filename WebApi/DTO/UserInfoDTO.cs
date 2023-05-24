using FluentValidation;

namespace WebApi.DTOs;

public class UserInfoDTO : BaseDTO
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
}

public class UserInfoDTOValidator : AbstractValidator<UserInfoDTO>
{
    public UserInfoDTOValidator()
    {
        RuleFor(x => x.Name).Name();
    }
}