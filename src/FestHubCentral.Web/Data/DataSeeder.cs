using System.Text.Json;
using System.Text.Json.Serialization;
using FestHubCentral.Web.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FestHubCentral.Web.Data;

public static class DataSeeder
{
    private static readonly DateTime SeedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new CustomDateTimeConverter() }
    };
    
    public static async Task SeedUsers(IServiceProvider serviceProvider, ApplicationDbContext context)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        const string adminEmail = "admin@festhub.ch";
        const string adminPassword = "Admin123!";

        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                DisplayName = "Administrator",
                EmailConfirmed = true,
                RequiresPasswordChange = true
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
        
        // Create an Account for each Location if not exists
        if (!await roleManager.RoleExistsAsync("Location"))
        {
            await roleManager.CreateAsync(new IdentityRole("Location"));
        }
        
        var locations = await context.Locations.ToListAsync();
        foreach (var loc in locations)
        {
            // replace all special characters in loc.Name to create a valid username
            var username = "location" + loc.Id;
            if (await userManager.FindByNameAsync(username) == null)
            {
                var locationUser = new ApplicationUser
                {
                    UserName = username,
                    Email = username + "@festhub.ch",
                    DisplayName = loc.Name,
                    EmailConfirmed = true,
                    RequiresPasswordChange = false,
                    LocationId = loc.Id,
                };

                var result = await userManager.CreateAsync(locationUser, "Start1234!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(locationUser, "Location");
                }
            }
        }
    }

    public static async Task SeedFromJsonFiles(ApplicationDbContext context)
    {
        var currentDir = Directory.GetCurrentDirectory();
        var backupPath = Path.Combine(currentDir, "backup_2024");

        if (!Directory.Exists(backupPath))
        {
            backupPath = Path.GetFullPath(Path.Combine(currentDir, "..", "..", "backup_2024"));
        }

        if (!Directory.Exists(backupPath))
        {
            Console.WriteLine($"Backup folder not found. Tried: {backupPath}");
            Console.WriteLine($"Current directory: {currentDir}");
            return;
        }

        Console.WriteLine($"Loading data from: {backupPath}");

        await SeedProducts(context, backupPath);
        await SeedInventoryTransfers(context, backupPath);
        await SeedProductLocationForecasts(context, backupPath);
        await SeedInventories(context, backupPath);

        await context.SaveChangesAsync();
    }

    private static async Task SeedProducts(ApplicationDbContext context, string backupPath)
    {
        var filePath = Path.Combine(backupPath, "Products.json");
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Products.json not found");
            return;
        }

        var json = await File.ReadAllTextAsync(filePath);
        var productsJson = JsonSerializer.Deserialize<List<ProductJson>>(json, JsonOptions);

        if (productsJson == null) return;

        foreach (var productJson in productsJson)
        {
            var existingProduct = await context.Products.FindAsync(productJson.Id);
            if (existingProduct != null) continue;

            var product = new Product
            {
                Id = productJson.Id,
                Name = productJson.ProduktName ?? "Unknown",
                Description = BuildProductDescription(productJson),
                Unit = "Bottle",
                SupplierId = productJson.SupplierId,
                IsAvailable = true,
                CreatedAt = SeedDate
            };

            context.Products.Add(product);

            if (productJson.Verkaufspreis.HasValue && productJson.Verkaufspreis > 0
                && productJson.Einkaufspreis1.HasValue && productJson.Einkaufspreis1 > 0)
            {
                var existingPrice = await context.ProductEventPrices
                    .FirstOrDefaultAsync(p => p.ProductId == productJson.Id && p.EventYear == 2024);

                if (existingPrice == null)
                {
                    var productEventPrice = new ProductEventPrice
                    {
                        ProductId = productJson.Id,
                        EventYear = 2024,
                        PurchasePrice = productJson.Einkaufspreis1.Value,
                        SellingPrice = productJson.Verkaufspreis.Value,
                        SpecialPrice = productJson.Einkaufspreis2.HasValue && productJson.Einkaufspreis2 > 0
                            ? productJson.Einkaufspreis2
                            : null,
                        CreatedAt = SeedDate
                    };
                    context.ProductEventPrices.Add(productEventPrice);
                }
            }
        }

        await context.SaveChangesAsync();
        Console.WriteLine($"Imported {productsJson.Count} products");
    }

    private static async Task SeedInventoryTransfers(ApplicationDbContext context, string backupPath)
    {
        var filePath = Path.Combine(backupPath, "InventoryTransfers.json");
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"InventoryTransfers.json not found");
            return;
        }

        var json = await File.ReadAllTextAsync(filePath);
        var transfersJson = JsonSerializer.Deserialize<List<InventoryTransferJson>>(json, JsonOptions);

        if (transfersJson == null) return;

        foreach (var transferJson in transfersJson)
        {
            var existingTransfer = await context.InventoryTransfers.FindAsync(transferJson.Id);
            if (existingTransfer != null) continue;

            var transfer = new InventoryTransfer
            {
                Id = transferJson.Id,
                ProductId = transferJson.ProductId,
                FromLocationId = transferJson.FromLocationId == 28 ? null : transferJson.FromLocationId,
                ToLocationId = transferJson.LocationId == 28 ? null : transferJson.LocationId,
                Amount = transferJson.Amount,
                Comment = transferJson.Comment,
                EventYear = 2024,
                TransferDate = transferJson.CreatedAt,
                CreatedAt = transferJson.CreatedAt
            };

            context.InventoryTransfers.Add(transfer);
        }

        await context.SaveChangesAsync();
        Console.WriteLine($"Imported {transfersJson.Count} inventory transfers");
    }

    private static async Task SeedProductLocationForecasts(ApplicationDbContext context, string backupPath)
    {
        var filePath = Path.Combine(backupPath, "ProductLocationForecasts.json");
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"ProductLocationForecasts.json not found");
            return;
        }

        var json = await File.ReadAllTextAsync(filePath);
        var forecastsJson = JsonSerializer.Deserialize<List<ProductLocationForecastJson>>(json, JsonOptions);

        if (forecastsJson == null) return;

        var validLocationIds = await context.Locations.Select(l => l.Id).ToListAsync();
        int importedCount = 0;
        int skippedCount = 0;

        foreach (var forecastJson in forecastsJson)
        {
            if (!forecastJson.LocationId.HasValue ||
                forecastJson.LocationId == 28 ||
                !validLocationIds.Contains(forecastJson.LocationId.Value) ||
                forecastJson.Amount <= 0)
            {
                skippedCount++;
                continue;
            }

            var existingProductLocation = await context.ProductLocations
                .FirstOrDefaultAsync(f => f.ProductId == forecastJson.ProductId
                                      && f.LocationId == forecastJson.LocationId
                                      && f.EventYear == 2024);

            if (existingProductLocation != null) continue;

            var productLocation = new ProductLocation
            {
                ProductId = forecastJson.ProductId,
                LocationId = forecastJson.LocationId.Value,
                PlannedAmount = forecastJson.Amount,
                InitialDelivery = forecastJson.InitialDelivery,
                Notes = forecastJson.Bemerkung,
                EventYear = 2024,
                CreatedAt = SeedDate
            };

            context.ProductLocations.Add(productLocation);
            importedCount++;
        }

        await context.SaveChangesAsync();
        Console.WriteLine($"Imported {importedCount} product locations (skipped {skippedCount} with invalid locations)");
    }

    private static async Task SeedInventories(ApplicationDbContext context, string backupPath)
    {
        var filePath = Path.Combine(backupPath, "ProductLocationForecasts.json");
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"ProductLocationForecasts.json not found");
            return;
        }

        var json = await File.ReadAllTextAsync(filePath);
        var forecastsJson = JsonSerializer.Deserialize<List<ProductLocationForecastJson>>(json, JsonOptions);

        if (forecastsJson == null) return;

        foreach (var forecastJson in forecastsJson)
        {
            var existingInventory = await context.Inventories
                .FirstOrDefaultAsync(i => i.ProductId == forecastJson.ProductId
                                  && i.EventYear == 2024);

            if (existingInventory != null) continue;

            var inventory = new Inventory
            {
                ProductId = forecastJson.ProductId,
                EventYear = 2024,
                CreatedAt = SeedDate
            };

            context.Inventories.Add(inventory);
        }

        await context.SaveChangesAsync();
        Console.WriteLine($"Imported {forecastsJson.Count} inventory records");
    }

    private static string BuildProductDescription(ProductJson productJson)
    {
        var parts = new List<string>();

        if (!string.IsNullOrEmpty(productJson.ProduktTyp))
            parts.Add(productJson.ProduktTyp);

        if (!string.IsNullOrEmpty(productJson.Gebinde) && !string.IsNullOrEmpty(productJson.GebindeTyp))
            parts.Add($"{productJson.Gebinde}{productJson.GebindeTyp}");

        if (!string.IsNullOrEmpty(productJson.Bemerkung))
            parts.Add($"Note: {productJson.Bemerkung}");

        return parts.Count > 0 ? string.Join(" - ", parts) : "";
    }

    private class ProductJson
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public string? ProduktName { get; set; }
        public string? ProduktTyp { get; set; }
        public string? Gebinde { get; set; }
        public string? GebindeTyp { get; set; }
        public string? Bemerkung { get; set; }
        public string? ArtikelNr { get; set; }
        public string? UID { get; set; }
        public string? Lieferant { get; set; }
        public decimal? Verkaufspreis { get; set; }
        public decimal? Einkaufspreis1 { get; set; }
        public decimal? Einkaufspreis2 { get; set; }
        public decimal? Einkaufspreis3 { get; set; }
        public int? Verbrauch2017_Total { get; set; }
        public int? Verbrauch2019_Total { get; set; }
        public int? Verbrauch2022_Total { get; set; }
        public int? Bestellen2024Ackerstein { get; set; }
        public int? Bestellen2024Kirche { get; set; }
    }

    private class InventoryTransferJson
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int? LocationId { get; set; }
        public int Amount { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? FromLocationId { get; set; }
    }

    private class ProductLocationForecastJson
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int? LocationId { get; set; }
        public int Amount { get; set; }
        public string? Bemerkung { get; set; }
        public int InitialDelivery { get; set; }
    }

    private class CustomDateTimeConverter : JsonConverter<DateTime>
    {
        private static readonly string[] DateFormats =
        {
            "yyyy-MM-dd HH:mm:ss.fffffff",
            "yyyy-MM-dd HH:mm:ss.ffffff",
            "yyyy-MM-dd HH:mm:ss.fffff",
            "yyyy-MM-dd HH:mm:ss.ffff",
            "yyyy-MM-dd HH:mm:ss.fff",
            "yyyy-MM-dd HH:mm:ss",
            "yyyy-MM-ddTHH:mm:ss.fffffffZ",
            "yyyy-MM-ddTHH:mm:ss.ffffffZ",
            "yyyy-MM-ddTHH:mm:ssZ"
        };

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dateString = reader.GetString();
            if (string.IsNullOrEmpty(dateString))
                return DateTime.MinValue;

            foreach (var format in DateFormats)
            {
                if (DateTime.TryParseExact(dateString, format, null, System.Globalization.DateTimeStyles.None, out var date))
                {
                    return DateTime.SpecifyKind(date, DateTimeKind.Utc);
                }
            }

            if (DateTime.TryParse(dateString, out var parsedDate))
            {
                return DateTime.SpecifyKind(parsedDate, DateTimeKind.Utc);
            }

            throw new JsonException($"Unable to parse date: {dateString}");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss.fffffff"));
        }
    }
}
