using Microsoft.Extensions.DependencyInjection;

public static class Helpers
{
    public static IServiceCollection AddHelpers(this IServiceCollection services, IHostEnvironment environment)
    {
        services.AddAutoMapper(typeof(Program));
        services.AddPropertyValidator();
        services.AddJWT();

        return services;
    }
}