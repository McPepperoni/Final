namespace FluentValidation;

public static class UserInfoValidators
{
    public static IRuleBuilderOptions<T, string>
           PhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.NotEmpty()
        .Matches(@"^0\d{3}\.\d{3}\.\d{3}").WithMessage("Phonenumber does not match format");
    }

    public static IRuleBuilderOptions<T, string> Name<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.NotEmpty();
    }

    public static IRuleBuilderOptions<T, string> Address<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.NotEmpty();
    }
}