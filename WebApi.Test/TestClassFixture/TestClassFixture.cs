using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Persistence;
using Persistence.Entities;
using Persistence.Managers;
using WebApi.Settings;
using WebApi.Test.Factory;

namespace WebApi.Test.ClassFixture;

public class TestClassFixture : IClassFixture<TestWebApplicationFactory<Program>>
{
    protected readonly TestWebApplicationFactory<Program> _factory;
    protected readonly HttpClient _client;
    protected UserEntity AdminUser;
    protected UserEntity User;
    protected readonly string _JWTKey;

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

    protected TestClassFixture(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;

        var scope = _factory.Services.CreateScope();
        var settings = scope.ServiceProvider.GetRequiredService<AppSettings>();

        _JWTKey = settings.JWT.Key;

        _client = _factory.CreateClient();
        _client.BaseAddress = new Uri(_client.BaseAddress.ToString() + "api/v1/");
        SeedData();
    }

    protected void SeedData()
    {
        Console.WriteLine("Begin seeding data...");
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();

        var RoleAdmin = new IdentityRole<Guid>();
        var RoleUser = new IdentityRole<Guid>();
        if (!dbContext.Roles.Any())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var roles = GetJson<IdentityRole<Guid>>(@"Db/RoleData.json");

            foreach (var role in roles)
            {
                role.NormalizedName = role.Name;
                dbContext.Roles.Add(role);
                if (role.Name == "Admin")
                {
                    RoleAdmin = role;
                }
                else
                {
                    RoleUser = role;
                }
            }
        }

        if (!dbContext.Users.Any())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<FinalUserManager>();
            var passwordHasher = new PasswordHasher<UserEntity>();

            var users = GetJson<UserSeedDTO>(@"Db/UserData.json");

            var mapperConfig = new MapperConfiguration(x =>
            {
                x.CreateMap<UserSeedDTO, UserEntity>();
            });
            var mapper = new Mapper(mapperConfig);

            foreach (var user in users)
            {
                var mappedUser = mapper.Map<UserEntity>(user);
                mappedUser.PasswordHash = passwordHasher.HashPassword(mappedUser, user.Password);
                mappedUser.NormalizedEmail = mappedUser.Email.ToUpper();
                mappedUser.NormalizedUserName = mappedUser.Email.ToUpper();

                dbContext.Users.Add(mappedUser);

                var role = dbContext.Roles.FirstOrDefault(x => x.Name == user.Role);
                var userRole = new IdentityUserRole<Guid>()
                {
                    UserId = mappedUser.Id,
                    RoleId = user.Role == "Admin" ? RoleAdmin.Id : RoleUser.Id,
                };
                dbContext.UserRoles.Add(userRole);
                if (user.Role == "Admin")
                {
                    AdminUser = mappedUser;
                }
                else
                {
                    User = mappedUser;
                }
            }
        }

        if (!dbContext.Products.Any())
        {
            var categories = GetJson<CategoryEntity>(@"Db/CategoryData.json");
            var products = GetJson<ProductEntity>(@"Db/ProductData.json");

            foreach (var item in categories)
            {
                dbContext.Categories.Add(item);
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

                dbContext.Products.Add(item);
            }

        }

        dbContext.SaveChanges();
        Console.WriteLine("Seeding data completed...");
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

    public static JWTTokenEntity Create(UserEntity user, string secretKey, string userRole)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var roles = new List<string> { userRole };

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("fullName", user.FullName),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = new JwtSecurityToken(
            "*",
            "*",
            claims,
            null,
            DateTime.UtcNow.AddHours(2),
            creds
        );

        return new JWTTokenEntity()
        {
            Token = tokenHandler.WriteToken(token),
            Expires = DateTime.UtcNow.AddHours(2),
        };
    }
}