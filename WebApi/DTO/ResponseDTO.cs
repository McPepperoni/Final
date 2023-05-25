namespace WebApi.DTOs;

public class LoginResponseDTO
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public UserDTO User { get; set; }
}

public class SearchResultDTO<T> where T : class
{
    public int Total { get; set; }
    public T Data { get; set; }
}