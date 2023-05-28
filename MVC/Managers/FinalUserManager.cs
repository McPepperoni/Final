using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MVC.Models;

namespace MVC.Managers;

public class FinalUserManager : UserManager<UserEntity>
{
    public FinalUserManager(IUserStore<UserEntity> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<UserEntity> passwordHasher, IEnumerable<IUserValidator<UserEntity>> userValidators, IEnumerable<IPasswordValidator<UserEntity>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<UserEntity>> logger) :
    base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {

    }

    public override async Task<IdentityResult> CreateAsync(UserEntity user, string password) {
        
        return await base.CreateAsync(user, password);
    }
}