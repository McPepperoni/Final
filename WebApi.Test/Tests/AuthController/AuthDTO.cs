using WebApi.DTOs;
using WebApi.Helpers.JWT;

namespace WebApi.Test.DTO;

public class AuthDTO
{
    public static UserSignUpDTO SignUp_OK() => new()
    {
        UserName = "TestAccount123",
        Password = "Thehpteam_16",
        Email = "team.building123@yahoo.com",
        FirstName = "Admin",
        LastName = "admin",
        PhoneNumber = "0913.501.314",
        Address = "46a Le Trung Nghia",
    };

    public static UserSignUpDTO SignUp_BadRequest() => new()
    {
        UserName = "TestAccount123",
        Password = "12345678",
        Email = "team.building123@yahoo.com",
        FirstName = "Admin",
        LastName = "admin",
        PhoneNumber = "0913.501.314",
        Address = "46a Le Trung Nghia",
    };

    public static UserSignUpDTO SignIn_OK() => new()
    {
        Email = "quangnguyen16200@gmail.com",
        Password = "Thehpteam_16"

    };

    public static UserSignUpDTO SignIn_BadRequest() => new()
    {
        Email = "quangnguyen16200@gmail.com",
        Password = "Thehpteam_15"
    };

    public static RefreshTokenDTO RefreshToken_OK() => new()
    {
        RefreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiQXV0aFRva2VuIiwianRpIjoiODdmZDM4NGQtN2E2Mi00Nzc4LWI5ZWYtY2IyMDcyNDFiY2Q5Iiwic3ViIjoiODg4YTY1ZGUtNjZlNy00NTRhLTg2YWItZmRiODAwZGUyZDA5IiwiUm9sZXMiOiJ1c2VyIiwiZXhwIjoyMDAwMDAwMDAwLCJpc3MiOiIqIiwiYXVkIjoiKiJ9.rG1EvXRkj8NihTX2afElkIY5flJFZiKyYXSmffnzGBY",
        AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiQXV0aFRva2VuIiwianRpIjoiODdmZDM4NGQtN2E2Mi00Nzc4LWI5ZWYtY2IyMDcyNDFiY2Q5Iiwic3ViIjoiODg4YTY1ZGUtNjZlNy00NTRhLTg2YWItZmRiODAwZGUyZDA5IiwiUm9sZXMiOiJ1c2VyIiwiZXhwIjoxNjg0ODU2Nzg0LCJpc3MiOiIqIiwiYXVkIjoiKiJ9.MYM6URb9d2JB8Cg9F7mlS_HFIRfeaotagn5nJgJSt80"
    };
}