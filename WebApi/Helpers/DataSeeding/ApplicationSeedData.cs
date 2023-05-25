using Microsoft.OpenApi.Writers;
using Newtonsoft.Json;
using WebApi.Contexts;
using WebApi.Entities;

namespace WebApi.Helpers.DataSeeding;

public class ApplicationSeedData
{
    public async static Task EnsureDataAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        // await dbContext.Database.MigrateAsync();
        if (!dbContext.Users.Any())
        {
            var users = SeedData();
            foreach (var item in users)
            {
                item.Password = BCrypt.Net.BCrypt.HashPassword(item.Password, 11, true);
                item.Cart = new();
                await dbContext.Users.AddAsync(item);
            }
            await dbContext.SaveChangesAsync();
        }
    }

    public static List<UserEntity> SeedData()
    {
        List<UserEntity> items = new List<UserEntity>();

        using (StreamReader r = new StreamReader(@"Db/UserData.json"))
        {
            string json = r.ReadToEnd();
            items = JsonConvert.DeserializeObject<List<UserEntity>>(json);
            return items;
        }
    }
}