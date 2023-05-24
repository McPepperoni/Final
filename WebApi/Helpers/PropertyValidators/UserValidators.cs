namespace FluentValidation;

public static class UserValidators
{
    public static IRuleBuilderOptions<T, string>
           Password<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.NotEmpty()
        .MinimumLength(6).WithMessage("Password must be longer than 6 character")
        .Matches("[A-Z]").WithMessage("Password must contains uppercase")
        .Matches("[a-z]").WithMessage("Password must contains lowercase")
        .Matches("[0-9]").WithMessage("Password must contains digit")
        .Matches("[^a-zA-Z0-9]").WithMessage("Password must contains special character");
    }

    public static IRuleBuilderOptions<T, string> Email<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.NotEmpty()
        .EmailAddress().WithMessage("Email Address is not correct");
    }
}