using MVC.Settings;
using Persistence.Managers;

namespace Microsoft.Extensions.DependencyInjection;

public static class Pages
{
    public static IServiceCollection AddPages(this IServiceCollection services)
    {
        services.AddRazorPages()
                .AddMvcOptions(options =>
                {
                });

        return services;
    }
}