using FluentValidation;
using FluentValidation.AspNetCore;
using WebApi.DTOs;

namespace Microsoft.Extensions.DependencyInjection;

public static class PropertyValidators
{
    public static IServiceCollection AddPropertyValidator(this IServiceCollection services, Action<FluentValidationAutoValidationConfiguration> configurationExpression = null)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<UserDTO>();

        return services;
    }
}