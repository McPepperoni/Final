using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApi.Constants;
using WebApi.Entities;
using WebApi.Settings;

namespace WebApi.Contexts;

public class ApplicationDbContext : DbContext
{
    private readonly AppSettings _appSettings;
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<UserInfoEntity> UserInfos { get; set; }
    public DbSet<JWTTokenEntity> WhiteListedToken { get; set; }

    public ApplicationDbContext(AppSettings appSettings)
    {
        _appSettings = appSettings;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_appSettings.ConnectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // User
        builder.Entity<UserEntity>()
        .ToTable("Users");

        builder.Entity<UserEntity>()
        .ToTable("Users").Property(x => x.CreatedAt);

        builder.Entity<UserEntity>()
        .Property(x => x.Role).HasDefaultValue(Roles.USER);

        builder.Entity<UserEntity>()
        .HasOne(x => x.UserInfo)
        .WithOne()
        .HasForeignKey<UserInfoEntity>()
        .OnDelete(DeleteBehavior.Cascade);

        // User Entity
        builder.Entity<UserInfoEntity>()
        .ToTable("UserInfos");

        //JWT Token
        builder.Entity<JWTTokenEntity>()
        .ToTable("WhiteListedToken");
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entities = ChangeTracker.Entries()
                                    .Where(x => x.Entity is BaseEntity && (x.State == EntityState.Modified || x.State == EntityState.Added));

        foreach (var entity in entities)
        {
            var now = DateTime.UtcNow;
            ((BaseEntity)entity.Entity).ModifiedAt = now;

            if (entity.State == EntityState.Added)
            {
                ((BaseEntity)entity.Entity).CreatedAt = now;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        var entities = ChangeTracker.Entries()
                                    .Where(x => x.Entity is BaseEntity && (x.State == EntityState.Modified || x.State == EntityState.Added));

        foreach (var entity in entities)
        {
            var now = DateTime.UtcNow;
            ((BaseEntity)entity.Entity).ModifiedAt = now;

            if (entity.State == EntityState.Added)
            {
                ((BaseEntity)entity.Entity).CreatedAt = now;
            }
        }

        return base.SaveChanges();
    }
}