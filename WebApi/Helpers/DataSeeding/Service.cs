using WebApi.Helpers.DataSeeding;

namespace Microsoft.Extensions.DependencyInjection;

public static class DataSeeder
{
    public static IServiceCollection AddDataSeeder(this IServiceCollection services)
    {
        services.AddTransient<ApplicationSeedData>();

        return services;
    }
}