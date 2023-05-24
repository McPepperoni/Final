using WebApi.Helpers.JWT;

namespace Microsoft.Extensions.DependencyInjection;

public static class JWT
{
    public static IServiceCollection AddJWT(this IServiceCollection services)
    {
        services.AddScoped<JWTHelper>();

        return services;
    }
}