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

    public DbSet<Event> Events => Set<Event>();
    public DbSet<Vendor> Vendors => Set<Vendor>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductEventPrice> ProductEventPrices => Set<ProductEventPrice>();
    public DbSet<Inventory> Inventories => Set<Inventory>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Alert> Alerts => Set<Alert>();
    public DbSet<CashRegister> CashRegisters => Set<CashRegister>();
    public DbSet<Settings> Settings => Set<Settings>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Year);
            entity.Property(e => e.Name).IsRequired();

            entity.HasMany(e => e.Orders)
                .WithOne(e => e.Event)
                .HasForeignKey(e => e.EventYear)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Inventories)
                .WithOne(e => e.Event)
                .HasForeignKey(e => e.EventYear)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.ProductEventPrices)
                .WithOne(e => e.Event)
                .HasForeignKey(e => e.EventYear)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Vendor>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.LocationSpot).IsUnique();
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Category).IsRequired();

            entity.HasMany(e => e.Orders)
                .WithOne(e => e.Vendor)
                .HasForeignKey(e => e.VendorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();

            entity.HasMany(e => e.Products)
                .WithOne(e => e.Supplier)
                .HasForeignKey(e => e.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();

            entity.HasOne(e => e.Inventory)
                .WithOne(e => e.Product)
                .HasForeignKey<Inventory>(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ProductEventPrice>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ProductId, e.EventYear }).IsUnique();
            entity.Property(e => e.Price).HasPrecision(18, 2);
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

        modelBuilder.Entity<Settings>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.UpcomingEvent)
                .WithMany()
                .HasForeignKey(e => e.UpcomingEventYear)
                .OnDelete(DeleteBehavior.Restrict);
        });

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        var event2024 = new Event
        {
            Year = 2024,
            Name = "Höngger Wümmetfäscht",
            StartDate = new DateTime(2024, 09, 20, 18, 0, 0, DateTimeKind.Utc),
            EndDate = new DateTime(2024, 09, 22, 18, 0, 0, DateTimeKind.Utc),
            IsPassed = true,
            Description = "45. Höngger Wümmetfäscht",
            CreatedAt = seedDate
        };

        var event2026 = new Event
        {
            Year = 2026,
            Name = "Höngger Wümmetfäscht",
            StartDate = new DateTime(2026, 09, 25, 18, 0, 0, DateTimeKind.Utc),
            EndDate = new DateTime(2026, 09, 27, 18, 0, 0, DateTimeKind.Utc),
            IsPassed = false,
            Description = "46. Höngger Wümmetfäscht",
            CreatedAt = seedDate
        };

        modelBuilder.Entity<Event>().HasData(event2024, event2026);

        var vendors = new[]
        {
            new Vendor { Id = 15, Name = "Wiilaube", Category = "Mixed", LocationSpot = 1, CreatedAt = seedDate },
            new Vendor { Id = 16, Name = "Raclett-Zelt", Category = "Mixed", LocationSpot = 2, CreatedAt = seedDate },
            new Vendor { Id = 17, Name = "Winzerlounge", Category = "Mixed", LocationSpot = 3, CreatedAt = seedDate },
            new Vendor { Id = 18, Name = "Wümmetkafi", Category = "Mixed", LocationSpot = 4, CreatedAt = seedDate },
            new Vendor { Id = 19, Name = "Wurst/Getränke Kirche", Category = "Mixed", LocationSpot = 5, CreatedAt = seedDate },
            new Vendor { Id = 20, Name = "Wurst/Getränke Ackersteinstrasse", Category = "Mixed", LocationSpot = 6, CreatedAt = seedDate },
            new Vendor { Id = 21, Name = "Bar Ackersteinstrasse", Category = "Mixed", LocationSpot = 7, CreatedAt = seedDate },
            new Vendor { Id = 22, Name = "Bar Mühlehalde", Category = "Mixed", LocationSpot = 8, CreatedAt = seedDate },
            new Vendor { Id = 23, Name = "Kiwanis", Category = "Mixed", LocationSpot = 9, CreatedAt = seedDate },
            new Vendor { Id = 24, Name = "Rebhüsli", Category = "Mixed", LocationSpot = 10, CreatedAt = seedDate },
            new Vendor { Id = 25, Name = "Fischstand", Category = "Mixed", LocationSpot = 11, CreatedAt = seedDate },
            new Vendor { Id = 26, Name = "OK-Ackerstein", Category = "Mixed", LocationSpot = 12, CreatedAt = seedDate },
            new Vendor { Id = 27, Name = "OK-Kirche", Category = "Mixed", LocationSpot = 13, CreatedAt = seedDate },
            new Vendor { Id = 28, Name = "Lieferant", Category = "Mixed", LocationSpot = 14, CreatedAt = seedDate },
            new Vendor { Id = 29, Name = "Crêpes Stand", Category = "Mixed", LocationSpot = 15, CreatedAt = seedDate },
            new Vendor { Id = 30, Name = "Suuserwagen", Category = "Mixed", LocationSpot = 16, CreatedAt = seedDate }
        };

        modelBuilder.Entity<Vendor>().HasData(vendors);

        var suppliers = new[]
        {
            new Supplier { Id = 1, Name = "Local Meat Supplier", ContactPerson = "John Doe", ContactPhone = "+41 44 123 4567", CreatedAt = seedDate },
            new Supplier { Id = 2, Name = "Beverages AG", ContactPerson = "Jane Smith", ContactPhone = "+41 44 234 5678", CreatedAt = seedDate },
            new Supplier { Id = 3, Name = "Bakery & Pizza Co", ContactPerson = "Mike Johnson", ContactPhone = "+41 44 345 6789", CreatedAt = seedDate }
        };

        modelBuilder.Entity<Supplier>().HasData(suppliers);

        var products = new[]
        {
            new Product { Id = 1, Name = "Cheeseburger", Unit = "Piece", SupplierId = 1, IsAvailable = true, CreatedAt = seedDate },
            new Product { Id = 2, Name = "French Fries", Unit = "Portion", SupplierId = 1, IsAvailable = true, CreatedAt = seedDate },
            new Product { Id = 3, Name = "Draft Beer", Unit = "Glass", SupplierId = 2, IsAvailable = true, CreatedAt = seedDate },
            new Product { Id = 4, Name = "Margherita Pizza", Unit = "Piece", SupplierId = 3, IsAvailable = true, CreatedAt = seedDate }
        };

        modelBuilder.Entity<Product>().HasData(products);

        var productEventPrices = new[]
        {
            new ProductEventPrice { Id = 1, ProductId = 1, EventYear = 2024, Price = 8.00m, CreatedAt = seedDate },
            new ProductEventPrice { Id = 2, ProductId = 2, EventYear = 2024, Price = 3.50m, CreatedAt = seedDate },
            new ProductEventPrice { Id = 3, ProductId = 3, EventYear = 2024, Price = 5.50m, CreatedAt = seedDate },
            new ProductEventPrice { Id = 4, ProductId = 4, EventYear = 2024, Price = 11.00m, CreatedAt = seedDate },
            new ProductEventPrice { Id = 5, ProductId = 1, EventYear = 2026, Price = 8.50m, CreatedAt = seedDate },
            new ProductEventPrice { Id = 6, ProductId = 2, EventYear = 2026, Price = 4.00m, CreatedAt = seedDate },
            new ProductEventPrice { Id = 7, ProductId = 3, EventYear = 2026, Price = 6.00m, CreatedAt = seedDate },
            new ProductEventPrice { Id = 8, ProductId = 4, EventYear = 2026, Price = 12.00m, CreatedAt = seedDate }
        };

        modelBuilder.Entity<ProductEventPrice>().HasData(productEventPrices);

        var inventories = new[]
        {
            new Inventory { Id = 1, ProductId = 1, CurrentStock = 50, MinimumStock = 10, MaximumStock = 100, ReorderQuantity = 50, EventYear = 2026, LastRestocked = seedDate, CreatedAt = seedDate },
            new Inventory { Id = 2, ProductId = 2, CurrentStock = 75, MinimumStock = 20, MaximumStock = 150, ReorderQuantity = 75, EventYear = 2026, LastRestocked = seedDate, CreatedAt = seedDate },
            new Inventory { Id = 3, ProductId = 3, CurrentStock = 8, MinimumStock = 15, MaximumStock = 200, ReorderQuantity = 100, EventYear = 2026, LastRestocked = seedDate, CreatedAt = seedDate },
            new Inventory { Id = 4, ProductId = 4, CurrentStock = 30, MinimumStock = 5, MaximumStock = 50, ReorderQuantity = 25, EventYear = 2026, LastRestocked = seedDate, CreatedAt = seedDate }
        };

        modelBuilder.Entity<Inventory>().HasData(inventories);

        var brandingSettings = new Settings
        {
            Id = 1,
            FestivalName = "FestHub Central",
            PrimaryColor = "#6366f1",
            SecondaryColor = "#8b5cf6",
            AccentColor = "#ec4899",
            Tagline = "Real-time festival gastronomy management",
            UpcomingEventYear = 2026,
            UpdatedAt = seedDate
        };

        modelBuilder.Entity<Settings>().HasData(brandingSettings);
    }
}
