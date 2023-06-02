namespace FluentValidation;

public static class BaseValidator
{
    public static IRuleBuilderOptions<T, string>
           Id<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.NotEmpty().Length(36);
    }
}