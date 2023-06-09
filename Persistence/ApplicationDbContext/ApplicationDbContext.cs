using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence;

public class ApplicationDbContext : IdentityDbContext<UserEntity, IdentityRole<string>, string>
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
        .Property(x => x.Id)
        .ValueGeneratedOnAdd();

        builder.Entity<UserEntity>()
        .HasOne(x => x.Cart)
        .WithOne()
        .HasForeignKey<CartEntity>(x => x.UserId)
        .OnDelete(DeleteBehavior.Cascade);

        // Category
        builder.Entity<CategoryEntity>()
        .ToTable("Categories");

        // Product
        builder.Entity<ProductEntity>()
        .ToTable("Products")
        .HasMany(x => x.Categories)
        .WithOne(x => x.Product)
        .HasForeignKey(x => x.ProductId)
        .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ProductEntity>()
        .HasMany<CartProductEntity>()
        .WithOne(x => x.Product)
        .HasForeignKey(x => x.ProductId)
        .OnDelete(DeleteBehavior.Cascade);

        // Product Category
        builder.Entity<ProductCategoryEntity>()
        .ToTable("ProductCategories");

        // Category
        builder.Entity<CategoryEntity>()
        .ToTable("Categories")
        .HasMany<ProductCategoryEntity>(x => x.ProductCategories)
        .WithOne(x => x.Category)
        .HasForeignKey(x => x.CategoryId)
        .OnDelete(DeleteBehavior.Cascade);

        // Cart Product
        builder.Entity<CartProductEntity>()
        .ToTable("CartProducts");


        // Cart
        builder.Entity<CartEntity>()
        .ToTable("Carts")
        .HasMany(x => x.CartProducts)
        .WithOne(x => x.Cart)
        .HasForeignKey(x => x.CartId)
        .OnDelete(DeleteBehavior.Cascade);

        // Order
        builder.Entity<OrderEntity>()
        .ToTable("Orders")
        .HasMany(x => x.Products)
        .WithOne(x => x.Order)
        .HasForeignKey(x => x.OrderId)
        .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<OrderEntity>()
        .HasOne(x => x.User)
        .WithMany(x => x.Orders)
        .HasForeignKey(x => x.UserId);

        // Order Product
        builder.Entity<OrderProductEntity>()
        .ToTable("OrderProduct");

        //JWT Token
        builder.Entity<JWTTokenEntity>()
        .ToTable("WhiteListedTokens");

        ConfigureSoftDeleteFilter(builder);
    }

    private static void ConfigureSoftDeleteFilter(ModelBuilder builder)
    {
        foreach (var softDeletableTypeBuilder in builder.Model.GetEntityTypes()
        .Where(x => typeof(ISoftDeletable).IsAssignableFrom(x.ClrType)))
        {
            var parameter = Expression.Parameter(softDeletableTypeBuilder.ClrType, "p");

            softDeletableTypeBuilder.SetQueryFilter(
                Expression.Lambda(
                    Expression.Equal(
                        Expression.Property(parameter, nameof(ISoftDeletable.DeletedAt)),
                        Expression.Constant(null)),
                    parameter)
            );
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entities = ChangeTracker.Entries()
                                    .Where(x => x.Entity is ITimestamp && (x.State == EntityState.Modified || x.State == EntityState.Added));

        foreach (var entity in entities)
        {
            var now = DateTime.UtcNow;
            ((ITimestamp)entity.Entity).ModifiedAt = now;

            if (entity.State == EntityState.Added)
            {
                ((ITimestamp)entity.Entity).CreatedAt = now;
            }
            else if (entity.State == EntityState.Modified)
            {
                ((ITimestamp)entity.Entity).ModifiedAt = now;
            }
            else if (entity.State == EntityState.Deleted)
            {
                entity.State = EntityState.Unchanged;
                ((ISoftDeletable)entity.Entity).DeletedAt = now;
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
            ((ITimestamp)entity.Entity).ModifiedAt = now;

            if (entity.State == EntityState.Added)
            {
                ((ITimestamp)entity.Entity).CreatedAt = now;
                ((ITimestamp)entity.Entity).ModifiedAt = now;
            }
            else if (entity.State == EntityState.Modified)
            {
                ((ITimestamp)entity.Entity).ModifiedAt = now;
            }
            else if (entity.State == EntityState.Deleted)
            {
                entity.State = EntityState.Unchanged;
                ((ITimestamp)entity.Entity).ModifiedAt = now;
                ((ITimestamp)entity.Entity).CreatedAt = now;
            }
        }

        return base.SaveChanges();
    }
}
