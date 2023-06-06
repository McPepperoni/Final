using Microsoft.AspNetCore.Identity;
using Persistence;
using Persistence.Entities;
using Persistence.Managers;

namespace Microsoft.Extensions.DependencyInjection;

public static class IdentityServices
{
    public static IServiceCollection AddIdentities(this IServiceCollection services)
    {
        services.AddIdentity<UserEntity, IdentityRole<string>>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddUserManager<FinalUserManager>()
        .AddSignInManager<FinalSignInManager>(); ;

        return services;
    }
}