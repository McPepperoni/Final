using System.Net;
using System.Net.Http.Json;
using WebApi.DTOs;
using WebApi.Test.ClassFixture;
using WebApi.Test.DTO;
using WebApi.Test.Factory;

namespace WebApi.Test;

public class AuthControllerTest : TestClassFixture
{
    public AuthControllerTest(TestWebApplicationFactory<Program> factory) : base(factory) { }

    [Fact]
    public async Task SignUp_ReturnsUserDTO()
    {
        //Arrange
        var userDto = AuthDTO.SignUp_OK();

        //Action
        var response = await _client.PostAsJsonAsync("Auth/signup", userDto);
        var user = await response.Content.ReadFromJsonAsync<UserDTO>();

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(user.Email, userDto.Email);
    }

    [Fact]
    public async Task SignUp_ReturnsBadRequest()
    {
        //Arrange
        var userDto = AuthDTO.SignUp_BadRequest();

        //Action
        var response = await _client.PostAsJsonAsync("Auth/signup", userDto);
        var user = await response.Content.ReadFromJsonAsync<UserDTO>();

        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task SignIn_ReturnsLoginResponseDTO()
    {
        var userDTO = AuthDTO.SignIn_OK();

        var response = await _client.PostAsJsonAsync("Auth/login", userDTO);
        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(loginResponse.User.Email, userDTO.Email);
    }

    [Fact]
    public async Task SignIn_ReturnsBadRequest()
    {
        var userDTO = AuthDTO.SignIn_BadRequest();

        var response = await _client.PostAsJsonAsync("Auth/login", userDTO);
        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();

        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task RefreshToken_OK()
    {
        var refreshToken = AuthDTO.RefreshToken_OK();

        var response = await _client.PostAsJsonAsync("Auth/refreshToken", refreshToken);
        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}