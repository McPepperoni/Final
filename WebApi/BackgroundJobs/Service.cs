using WebApi.BackgroundJobs;

namespace Microsoft.Extensions.DependencyInjection;

public static class BackgroundJobs
{
    public static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
    {
        services.AddHostedService<CleanToken>();

        return services;
    }
}