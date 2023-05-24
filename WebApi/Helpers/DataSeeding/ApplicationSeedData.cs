using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApi.Contexts;
using WebApi.Entities;

namespace WebApi.Helpers.DataSeeding;

public class ApplicationSeedData
{
    private readonly ApplicationDbContext _dbContext;
    public ApplicationSeedData(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public virtual async Task EnsureDataAsync()
    {
        // await _dbContext.Database.MigrateAsync();
        if (!_dbContext.Users.Any())
        {
            var users = SeedData();
            foreach (var item in users)
            {
                item.Password = BCrypt.Net.BCrypt.HashPassword(item.Password, 11, true);
                await _dbContext.Users.AddAsync(item);
            }
            await _dbContext.SaveChangesAsync();
        }
    }

    public virtual List<UserEntity> SeedData()
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