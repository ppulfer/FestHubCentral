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
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductEventPrice> ProductEventPrices => Set<ProductEventPrice>();
    public DbSet<Inventory> Inventories => Set<Inventory>();
    public DbSet<InventoryTransfer> InventoryTransfers => Set<InventoryTransfer>();
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

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.LocationSpot).IsUnique();
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Category).IsRequired();

            entity.HasMany(e => e.Orders)
                .WithOne(e => e.Location)
                .HasForeignKey(e => e.LocationId)
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
        });

        modelBuilder.Entity<ProductEventPrice>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ProductId, e.EventYear }).IsUnique();
            entity.Property(e => e.PurchasePrice).HasPrecision(18, 2);
            entity.Property(e => e.SellingPrice).HasPrecision(18, 2);
            entity.Property(e => e.SpecialPrice).HasPrecision(18, 2);
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ProductId, e.LocationId, e.EventYear }).IsUnique();

            entity.HasOne(e => e.Location)
                .WithMany()
                .HasForeignKey(e => e.LocationId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<InventoryTransfer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.EventYear, e.TransferDate });

            entity.HasOne(e => e.Product)
                .WithMany()
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.FromLocation)
                .WithMany()
                .HasForeignKey(e => e.FromLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ToLocation)
                .WithMany()
                .HasForeignKey(e => e.ToLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Event)
                .WithMany()
                .HasForeignKey(e => e.EventYear)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.CreatedByUser)
                .WithMany()
                .HasForeignKey(e => e.CreatedByUserId)
                .OnDelete(DeleteBehavior.SetNull);
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

            entity.HasOne(e => e.Location)
                .WithMany()
                .HasForeignKey(e => e.LocationId)
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

            entity.HasOne(e => e.Location)
                .WithMany()
                .HasForeignKey(e => e.LocationId)
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

        var locations = new[]
        {
            new Location { Id = 15, Name = "Wiilaube", Category = "Vendor", LocationSpot = 1, CreatedAt = seedDate },
            new Location { Id = 16, Name = "Raclett-Zelt", Category = "Vendor", LocationSpot = 2, CreatedAt = seedDate },
            new Location { Id = 17, Name = "Winzerlounge", Category = "Vendor", LocationSpot = 3, CreatedAt = seedDate },
            new Location { Id = 18, Name = "Wümmetkafi", Category = "Vendor", LocationSpot = 4, CreatedAt = seedDate },
            new Location { Id = 19, Name = "Wurst/Getränke Kirche", Category = "Vendor", LocationSpot = 5, CreatedAt = seedDate },
            new Location { Id = 20, Name = "Wurst/Getränke Ackersteinstrasse", Category = "Vendor", LocationSpot = 6, CreatedAt = seedDate },
            new Location { Id = 21, Name = "Bar Ackersteinstrasse", Category = "Vendor", LocationSpot = 7, CreatedAt = seedDate },
            new Location { Id = 22, Name = "Bar Mühlehalde", Category = "Vendor", LocationSpot = 8, CreatedAt = seedDate },
            new Location { Id = 23, Name = "Kiwanis", Category = "Vendor", LocationSpot = 9, CreatedAt = seedDate },
            new Location { Id = 24, Name = "Rebhüsli", Category = "Vendor", LocationSpot = 10, CreatedAt = seedDate },
            new Location { Id = 25, Name = "Fischstand", Category = "Vendor", LocationSpot = 11, CreatedAt = seedDate },
            new Location { Id = 26, Name = "OK-Ackerstein", Category = "Staging Area", LocationSpot = 12, CreatedAt = seedDate },
            new Location { Id = 27, Name = "OK-Kirche", Category = "Staging Area", LocationSpot = 13, CreatedAt = seedDate },
            new Location { Id = 29, Name = "Crêpes Stand", Category = "Vendor", LocationSpot = 15, CreatedAt = seedDate },
            new Location { Id = 30, Name = "Suuserwagen", Category = "Vendor", LocationSpot = 16, CreatedAt = seedDate }
        };

        modelBuilder.Entity<Location>().HasData(locations);

        var suppliers = new[]
        {
            new Supplier { Id = 11, Name = "Wegmann", CreatedAt = seedDate },
            new Supplier { Id = 12, Name = "Zweifel", CreatedAt = seedDate },
            new Supplier { Id = 13, Name = "Stadt Zürich", CreatedAt = seedDate },
            new Supplier { Id = 14, Name = "Top CC", CreatedAt = seedDate },
            new Supplier { Id = 17, Name = "Steiner", CreatedAt = seedDate },
            new Supplier { Id = 18, Name = "Angst", CreatedAt = seedDate },
            new Supplier { Id = 19, Name = "OK", CreatedAt = seedDate },
            new Supplier { Id = 20, Name = "Lenzlinger", CreatedAt = seedDate },
            new Supplier { Id = 22, Name = "Rausch Packaging", CreatedAt = seedDate }
        };

        modelBuilder.Entity<Supplier>().HasData(suppliers);

        var products = Array.Empty<Product>();

        modelBuilder.Entity<Product>().HasData(products);

        var productEventPrices = Array.Empty<ProductEventPrice>();

        modelBuilder.Entity<ProductEventPrice>().HasData(productEventPrices);

        var inventories = Array.Empty<Inventory>();

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
