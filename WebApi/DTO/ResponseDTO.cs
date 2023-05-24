namespace WebApi.DTOs;

public class WebSocketResponseDTO<T> where T : class
{
    public string status { get; set; }
    public T data { get; set; }
}

public class LoginResponseDTO
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public UserDTO User { get; set; }
}

public class VerifyCredentialResponseDTO
{
    public bool Email { get; set; } = true;
    public bool PhoneNumber { get; set; } = true;
}

public class SearchResultDTO<T> where T : class
{
    public int Total { get; set; }
    public T Data { get; set; }
}