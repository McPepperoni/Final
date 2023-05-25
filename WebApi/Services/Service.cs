using WebApi.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class Services
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJWTService, JWTService>();
        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}