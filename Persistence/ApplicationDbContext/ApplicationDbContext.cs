using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence;

public class ApplicationDbContext : IdentityDbContext<UserEntity, IdentityRole<Guid>, Guid>
{
    public DbSet<CartEntity> Carts { get; set; }
    public DbSet<CartProductEntity> CartProducts { get; set; }
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<OrderProductEntity> OrderProducts { get; set; }
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<ProductCategoryEntity> ProductCategories { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<JWTTokenEntity> WhiteListedToken { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<UserEntity>()
        .HasOne(x => x.Cart)
        .WithOne()
        .HasForeignKey<CartEntity>();

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
        .HasMany<ProductCategoryEntity>()
        .WithOne(x => x.Category)
        .OnDelete(DeleteBehavior.Cascade);

        // Cart Product
        builder.Entity<CartProductEntity>()
        .ToTable("CartProducts");


        // Cart
        builder.Entity<CartEntity>()
        .ToTable("Carts")
        .HasMany(x => x.CartProducts)
        .WithOne(x => x.Cart)
        .OnDelete(DeleteBehavior.Cascade);

        // Order
        builder.Entity<OrderEntity>()
        .ToTable("Orders")
        .HasMany(x => x.OrderProducts)
        .WithOne()
        .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<OrderEntity>()
        .HasOne(x => x.User)
        .WithMany();

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
