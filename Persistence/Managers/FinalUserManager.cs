using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Persistence;
using Persistence.Entities;

namespace Persistence.Managers;

public class FinalUserManager : UserManager<UserEntity>
{
    private readonly ApplicationDbContext _dbContext;
    public FinalUserManager(ApplicationDbContext dbContext, IUserStore<UserEntity> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<UserEntity> passwordHasher, IEnumerable<IUserValidator<UserEntity>> userValidators, IEnumerable<IPasswordValidator<UserEntity>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<UserEntity>> logger) :
    base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
        _dbContext = dbContext;
    }

    public override async Task<IdentityResult> CreateAsync(UserEntity user, string password)
    {
        user.UserName = user.Email;

        return await base.CreateAsync(user, password);
    }

    public async Task<IdentityResult> AssignUserToRole(UserEntity user, string roleName)
    {
        var role = await _dbContext.Roles.Where(x => x.Name == roleName).FirstOrDefaultAsync();

        if (role == null)
        {
            return IdentityResult.Failed(new IdentityError { Code = "404", Description = "No role found" });
        }

        await _dbContext.UserRoles.AddAsync(new IdentityUserRole<Guid>
        {
            UserId = user.Id,
            RoleId = role.Id,
        });

        await _dbContext.SaveChangesAsync();

        return IdentityResult.Success;
    }
}