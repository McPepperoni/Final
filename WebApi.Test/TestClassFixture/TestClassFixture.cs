using System.Net.Http.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

using WebApi.Contexts;
using WebApi.DTOs;
using WebApi.Entities;

using WebApi.Test.Factory;
using WebApi.Test.MappingProfiles;

namespace WebApi.Test.ClassFixture;

public class TestClassFixture : IClassFixture<TestWebApplicationFactory<Program>>
{
    protected readonly TestWebApplicationFactory<Program> _factory;
    protected readonly HttpClient _client;
    protected readonly IMapper _mapper;

    protected TestClassFixture(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;

        SeedData();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new UserControllerProfile());
        });

        _client = _factory.CreateClient();
        _client.BaseAddress = new Uri(_client.BaseAddress.ToString() + "api/v1/");
        _mapper = new Mapper(mapperConfig);
    }

    protected async Task<LoginResponseDTO> GetAuth()
    {
        var response = await _client.PostAsJsonAsync("Auth/login", new UserLoginDTO()
        {
            Email = "quangnguyen16200@gmail.com",
            Password = "Thehpteam_16",
        });

        return await response.Content.ReadFromJsonAsync<LoginResponseDTO>();
    }

    protected void SeedData()
    {
        using var scope = _factory.Services.CreateScope();
        using var _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var loggedUser = new UserEntity();

        _dbContext.Database.Migrate();

        if (!_dbContext.Users.Any())
        {
            var users = ReadJSON<List<UserEntity>>(@"Db/UserData.json");
            foreach (var item in users)
            {
                if (item.Id == "888a65de-66e7-454a-86ab-fdb800de2d09")
                {
                    loggedUser = item;
                }

                item.Password = BCrypt.Net.BCrypt.HashPassword(item.Password, 11, true);
                _dbContext.Users.Add(item);
            }
        }

        if (!_dbContext.WhiteListedToken.Any())
        {
            var tokens = ReadJSON<List<JWTTokenEntity>>(@"Db/JWTData.json");
            foreach (var item in tokens)
            {
                item.Expires = new DateTime(2603, 10, 11);
                item.User = loggedUser;
                _dbContext.WhiteListedToken.Add(item);
            }
        }
        _dbContext.SaveChanges();
    }

    private T ReadJSON<T>(string path)
    {
        T items;

        using (StreamReader r = new StreamReader(path))
        {
            string json = r.ReadToEnd();
            items = JsonConvert.DeserializeObject<T>(json);
            return items;
        }
    }
}