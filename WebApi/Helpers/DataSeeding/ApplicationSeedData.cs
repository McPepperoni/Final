using System.Runtime.CompilerServices;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Writers;
using Newtonsoft.Json;
using Persistence;
using Persistence.Entities;
using Persistence.Managers;

namespace WebApi.Helpers.DataSeeding;

public class ApplicationSeedData
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

    public async static Task EnsureDataAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (!dbContext.Roles.Any())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var roles = GetJson<IdentityRole<Guid>>(@"Db/RoleData.json");

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(role.Name));
            }
        }

        if (!dbContext.Users.Any())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<FinalUserManager>();

            var users = GetJson<UserSeedDTO>(@"Db/UserData.json");

            var mapperConfig = new MapperConfiguration(x =>
            {
                x.CreateMap<UserSeedDTO, UserEntity>();
            });
            var mapper = new Mapper(mapperConfig);

            foreach (var user in users)
            {
                var mappedUser = mapper.Map<UserEntity>(user);
                var result = await userManager.CreateAsync(mappedUser, user.Password);
                result = await userManager.AddToRoleAsync(mappedUser, user.Role);
            }
        }

        if (!dbContext.Products.Any())
        {
            var categories = GetJson<CategoryEntity>(@"Db/CategoryData.json");
            var products = GetJson<ProductEntity>(@"Db/ProductData.json");

            foreach (var item in categories)
            {
                await dbContext.Categories.AddAsync(item);
            }

            foreach (var item in products)
            {
                item.Categories = new();
                var random = new Random();
                for (int i = 0; i < random.Next(2, 5); i++)
                {
                    var productCategory = new ProductCategoryEntity()
                    {
                        Category = categories[random.Next(categories.Count)]
                    };

                    item.Categories.Add(productCategory);
                }
                item.ImgSrc = $"https://source.unsplash.com/random/800x800/?img={random.Next(20)}";

                await dbContext.Products.AddAsync(item);
            }

        }

        await dbContext.SaveChangesAsync();
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