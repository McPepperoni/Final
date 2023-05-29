namespace MVC.Settings;

public class AppSettings
{
    public string ConnectionString { get; set; }
    public string ProductAPIUri { get; set; }

    private readonly IConfiguration _configuration;

    public AppSettings(IConfiguration configuration)
    {
        _configuration = configuration;

        ConnectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        ProductAPIUri = configuration.GetConnectionString("ProductAPI");
    }
}