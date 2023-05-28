using System.Runtime.CompilerServices;
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

            await dbContext.SaveChangesAsync();
        }
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