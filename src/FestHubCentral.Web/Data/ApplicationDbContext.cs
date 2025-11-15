using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FestHubCentral.Web.Data.Models;

namespace FestHubCentral.Web.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Vendor> Vendors => Set<Vendor>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Inventory> Inventories => Set<Inventory>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Alert> Alerts => Set<Alert>();
    public DbSet<CashRegister> CashRegisters => Set<CashRegister>();
    public DbSet<BrandingSettings> BrandingSettings => Set<BrandingSettings>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Vendor>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.LocationSpot).IsUnique();
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Category).IsRequired();

            entity.HasMany(e => e.Products)
                .WithOne(e => e.Vendor)
                .HasForeignKey(e => e.VendorId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Orders)
                .WithOne(e => e.Vendor)
                .HasForeignKey(e => e.VendorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Price).HasPrecision(18, 2);

            entity.HasOne(e => e.Inventory)
                .WithOne(e => e.Product)
                .HasForeignKey<Inventory>(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ProductId).IsUnique();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.OrderNumber).IsUnique();
            entity.Property(e => e.TotalAmount).HasPrecision(18, 2);

            entity.HasMany(e => e.OrderItems)
                .WithOne(e => e.Order)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
            entity.Property(e => e.Subtotal).HasPrecision(18, 2);

            entity.HasOne(e => e.Product)
                .WithMany(e => e.OrderItems)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Alert>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.IsResolved, e.CreatedAt });

            entity.HasOne(e => e.Vendor)
                .WithMany()
                .HasForeignKey(e => e.VendorId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<CashRegister>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OpeningAmount).HasPrecision(18, 2);
            entity.Property(e => e.ClosingAmount).HasPrecision(18, 2);
            entity.Property(e => e.ExpectedAmount).HasPrecision(18, 2);
            entity.Property(e => e.Discrepancy).HasPrecision(18, 2);
            entity.Property(e => e.CashSales).HasPrecision(18, 2);
            entity.Property(e => e.CardSales).HasPrecision(18, 2);
            entity.Property(e => e.TokenSales).HasPrecision(18, 2);

            entity.HasOne(e => e.Vendor)
                .WithMany()
                .HasForeignKey(e => e.VendorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<BrandingSettings>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        var vendors = new[]
        {
            new Vendor { Id = 1, Name = "Burger Stand", Category = "Food", LocationSpot = 1, IsOpen = true, CreatedAt = seedDate },
            new Vendor { Id = 2, Name = "Beer Garden", Category = "Beverage", LocationSpot = 2, IsOpen = true, CreatedAt = seedDate },
            new Vendor { Id = 3, Name = "Pizza Corner", Category = "Food", LocationSpot = 3, IsOpen = false, CreatedAt = seedDate }
        };

        modelBuilder.Entity<Vendor>().HasData(vendors);

        var products = new[]
        {
            new Product { Id = 1, Name = "Cheeseburger", Price = 8.50m, Unit = "Piece", VendorId = 1, IsAvailable = true, CreatedAt = seedDate },
            new Product { Id = 2, Name = "French Fries", Price = 4.00m, Unit = "Portion", VendorId = 1, IsAvailable = true, CreatedAt = seedDate },
            new Product { Id = 3, Name = "Draft Beer", Price = 6.00m, Unit = "Glass", VendorId = 2, IsAvailable = true, CreatedAt = seedDate },
            new Product { Id = 4, Name = "Margherita Pizza", Price = 12.00m, Unit = "Piece", VendorId = 3, IsAvailable = true, CreatedAt = seedDate }
        };

        modelBuilder.Entity<Product>().HasData(products);

        var inventories = new[]
        {
            new Inventory { Id = 1, ProductId = 1, CurrentStock = 50, MinimumStock = 10, MaximumStock = 100, ReorderQuantity = 50, LastRestocked = seedDate, CreatedAt = seedDate },
            new Inventory { Id = 2, ProductId = 2, CurrentStock = 75, MinimumStock = 20, MaximumStock = 150, ReorderQuantity = 75, LastRestocked = seedDate, CreatedAt = seedDate },
            new Inventory { Id = 3, ProductId = 3, CurrentStock = 8, MinimumStock = 15, MaximumStock = 200, ReorderQuantity = 100, LastRestocked = seedDate, CreatedAt = seedDate },
            new Inventory { Id = 4, ProductId = 4, CurrentStock = 30, MinimumStock = 5, MaximumStock = 50, ReorderQuantity = 25, LastRestocked = seedDate, CreatedAt = seedDate }
        };

        modelBuilder.Entity<Inventory>().HasData(inventories);

        var brandingSettings = new BrandingSettings
        {
            Id = 1,
            FestivalName = "FestHub Central",
            PrimaryColor = "#6366f1",
            SecondaryColor = "#8b5cf6",
            AccentColor = "#ec4899",
            Tagline = "Real-time festival gastronomy management",
            UpdatedAt = seedDate
        };

        modelBuilder.Entity<BrandingSettings>().HasData(brandingSettings);
    }
}
