using MVC.Settings;

namespace Microsoft.Extensions.DependencyInjection;

public static class HttpClients
{
    public static IServiceCollection AddHttpClients(this IServiceCollection services)
    {
        var scope = services.BuildServiceProvider().CreateScope();
        var appSettings = scope.ServiceProvider.GetRequiredService<AppSettings>();

        services.AddHttpClient("ProductAPIClient", cfg =>
        {
            cfg.BaseAddress = new Uri(appSettings.ProductAPIUri);
        });

        return services;
    }
}