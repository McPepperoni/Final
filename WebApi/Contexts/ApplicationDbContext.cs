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
    public DbSet<UserRoleEntity> UserRoles { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<CartEntity> Carts { get; set; }
    public DbSet<CartProductEntity> CartProducts { get; set; }
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<OrderProductEntity> OrderProducts { get; set; }
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<ProductCategoryEntity> ProductCategories { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }
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
        .HasMany(x => x.UserRoles)
        .WithOne(x => x.User)
        .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserEntity>()
        .HasOne(x => x.UserInfo)
        .WithOne(x => x.User)
        .HasForeignKey<UserInfoEntity>()
        .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserEntity>()
        .HasOne(x => x.Cart)
        .WithOne(x => x.User)
        .HasForeignKey<CartEntity>()
        .OnDelete(DeleteBehavior.Cascade);

        // User Info Entity
        builder.Entity<UserInfoEntity>()
        .ToTable("UserInfos");

        // User Role
        builder.Entity<UserRoleEntity>()
        .ToTable("UserRoles");

        // Role
        builder.Entity<RoleEntity>()
        .ToTable("Roles")
        .HasMany(x => x.UserRoles)
        .WithOne(x => x.Role)
        .OnDelete(DeleteBehavior.Cascade);

        // Category
        builder.Entity<CategoryEntity>()
        .ToTable("Categories");

        // Product
        builder.Entity<ProductEntity>()
        .ToTable("Products")
        .HasMany(x => x.Categories)
        .WithOne(x => x.Product)
        .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ProductEntity>()
        .HasMany<CartProductEntity>()
        .WithOne(x => x.Product)
        .OnDelete(DeleteBehavior.Cascade);

        // Product Category
        builder.Entity<ProductCategoryEntity>()
        .ToTable("ProductCategories");

        // Category
        builder.Entity<CategoryEntity>()
        .ToTable("Categories")
        .HasMany(x => x.ProductCategories)
        .WithOne(x => x.Category)
        .OnDelete(DeleteBehavior.Cascade);

        // Cart Product
        builder.Entity<CartProductEntity>()
        .ToTable("CartProducts");


        // Cart
        builder.Entity<CartEntity>()
        .ToTable("Carts")
        .HasMany(x => x.CartProducts)
        .WithOne()
        .OnDelete(DeleteBehavior.Cascade);

        // Order
        builder.Entity<OrderEntity>()
        .ToTable("Orders")
        .HasMany(x => x.OrderProducts)
        .WithOne()
        .OnDelete(DeleteBehavior.Cascade);

        // Order Product
        builder.Entity<OrderProductEntity>()
        .ToTable("OrderProduct");

        //JWT Token
        builder.Entity<JWTTokenEntity>()
        .ToTable("WhiteListedTokens");
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