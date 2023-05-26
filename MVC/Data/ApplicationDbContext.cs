using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVC.Models;
using MVC.Settings;

namespace MVC.Data;

public class ApplicationDbContext : IdentityDbContext<UserEntity, IdentityRole<Guid>, Guid>
{
    private readonly AppSettings _appSettings;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, AppSettings appSettings)
        : base(options)
    {
        _appSettings = appSettings;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_appSettings.ConnectionString);
        base.OnConfiguring(optionsBuilder);
    }
}
