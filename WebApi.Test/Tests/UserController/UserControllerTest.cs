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
        var user = await GetAuth();

        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {user.AccessToken}");

        var response = await _client.GetAsync("User");
        var users = await response.Content.ReadFromJsonAsync<PaginationResponseDTO<UserDTO>>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(10, users.Data.Count);
    }

    [Fact]
    public async Task GetUsers_ReturnsUnAuthorize()
    {
        var user = await GetAuth();

        var response = await _client.GetAsync("User");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}