using System.Net;
using System.Net.Http.Json;
using WebApi.DTOs;
using WebApi.Test.ClassFixture;
using WebApi.Test.Factory;

namespace WebApi.Test;

public class UserControllerTest : TestClassFixture
{
    public UserControllerTest(TestWebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetUsers_ReturnsOK()
    {
        var accessToken = Create(AdminUser, _JWTKey, "Admin");
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken.Token}");

        var response = await _client.GetAsync("User");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetUser_WithId_ReturnsOK()
    {
        var accessToken = Create(AdminUser, _JWTKey, "User");
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken.Token}");
        var response = await _client.GetAsync("User/e5ed5f2c-4d11-42d4-a840-2ba1d6e62c9a");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PostUser_ReturnsNoContent()
    {
        var user = new CreateUserDTO()
        {
            Email = "abc@cd.com",
            Password = "r6cC_&vI8k5NPP",
            FullName = "Filter hold",
            PhoneNumber = "0914678997",
            Address = "6237 shgdsg 438"
        };

        var response = await _client.PostAsJsonAsync("User", user);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task UpdateUser_ReturnNoContent()
    {
        var accessToken = Create(User, _JWTKey, "User");
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken.Token}");
        var user = new UpdateUserDTO()
        {
            Email = "abc@cd.com",
            FullName = "Filter hold",
            PhoneNumber = "0914678997",
            Address = "6237 shgdsg 438"
        };

        var response = await _client.PutAsJsonAsync("User/3b361729-1028-48dc-98c9-8535f88f87f1", user);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteUser_ReturnNoContent()
    {
        var accessToken = Create(AdminUser, _JWTKey, "Admin");
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken.Token}");

        var response = await _client.DeleteAsync("User/3b361729-1028-48dc-98c9-8535f88f87f1");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}