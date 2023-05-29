using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Persistence;
using Persistence.Entities;
using Persistence.Managers;

namespace MVC.Data;

public static class ApplicationSeedData
{
    public class UserSeedDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
    }

    public static async Task EnsureData(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if (!dbContext.Users.Any())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var userManager = scope.ServiceProvider.GetRequiredService<FinalUserManager>();

            var roles = GetJson<IdentityRole<Guid>>(@"Data/RoleData.json");
            var users = GetJson<UserSeedDTO>(@"Data/UserData.json");

            var mapperConfig = new MapperConfiguration(x =>
            {
                x.CreateMap<UserSeedDTO, UserEntity>();
            });
            var mapper = new Mapper(mapperConfig);

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(role.Name));
            }

            foreach (var user in users)
            {
                var mappedUser = mapper.Map<UserEntity>(user);
                var result = await userManager.CreateAsync(mappedUser, user.Password);
                result = await userManager.AddToRoleAsync(mappedUser, user.Role);
            }
        }
    }

    public static async Task<IdentityResult> AssignRoles(FinalUserManager userManager, string email, List<string> roles)
    {
        var user = await userManager.FindByEmailAsync(email);
        var result = await userManager.AddToRolesAsync(user, roles);

        return result;
    }

    private static List<T> GetJson<T>(string path)
    {
        List<T> items = new List<T>();

        using (StreamReader r = new StreamReader(path))
        {
            string json = r.ReadToEnd();
            items = JsonConvert.DeserializeObject<List<T>>(json);
            return items;
        }
    }
}