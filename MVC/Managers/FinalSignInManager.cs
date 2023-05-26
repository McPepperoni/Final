using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MVC.DTOs;
using MVC.Models;

namespace MVC.Managers;

public class FinalSignInManager : SignInManager<UserEntity>
{
    private readonly RoleManager<IdentityRole> _roleManager;
    public FinalSignInManager(UserManager<UserEntity> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<UserEntity> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<UserEntity>> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<UserEntity> confirmation, RoleManager<IdentityRole> roleManager) :
    base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
    {
        roleManager = _roleManager;
    }

    public async Task<SignInResult> SignInWithEmailPasswordAsync(string email, string password, bool isPersistent)
    {
        var user = await UserManager.FindByEmailAsync(email);
        if (user == null)
        {
            return SignInResult.Failed;
        }
        var roles = await UserManager.GetRolesAsync(user);

        var payload = new GetTokenDTO()
        {
            Id = user.Id.ToString(),
            Roles = roles.ToList()
        };

        return await base.PasswordSignInAsync(user, password, isPersistent, false);
    }
}