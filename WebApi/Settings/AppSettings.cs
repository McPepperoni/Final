namespace WebApi.Settings;

public class AppSettings
{
    private readonly IConfiguration _configuration;
    public string ConnectionString { get; private set; }
    public JWT JWT { get; set; }

    public AppSettings(IConfiguration configuration)
    {
        _configuration = configuration;

        ConnectionString = configuration.GetConnectionString("DefaultConnectionString") ?? "";
        JWT = configuration.GetSection("JWT").Get<JWT>();
    }
}