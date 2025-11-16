using FestHubCentral.Web.Data;
using FestHubCentral.Web.Data.Models;
using FestHubCentral.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FestHubCentral.Web.Services;

public class SettingsService : ISettingsService
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private static readonly SemaphoreSlim _semaphore = new(1, 1);
    private static Settings? _cachedSettings;
    private static DateTime _cacheExpiration = DateTime.MinValue;

    public SettingsService(ApplicationDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public async Task<Settings> GetSettingsAsync()
    {
        if (_cachedSettings != null && DateTime.UtcNow < _cacheExpiration)
        {
            return _cachedSettings;
        }

        await _semaphore.WaitAsync();
        try
        {
            if (_cachedSettings != null && DateTime.UtcNow < _cacheExpiration)
            {
                return _cachedSettings;
            }

            var settings = await _context.Settings
                .Include(s => s.UpcomingEvent)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (settings == null)
            {
                settings = new Settings
                {
                    FestivalName = "FestHub Central",
                    PrimaryColor = "#6366f1",
                    SecondaryColor = "#8b5cf6",
                    AccentColor = "#ec4899",
                    Tagline = "Real-time festival gastronomy management",
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Settings.Add(settings);
                await _context.SaveChangesAsync();
            }

            _cachedSettings = settings;
            _cacheExpiration = DateTime.UtcNow.AddMinutes(5);

            return settings;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<Settings> UpdateSettingsAsync(Settings settings)
    {
        await _semaphore.WaitAsync();
        try
        {
            var existing = await _context.Settings.FirstOrDefaultAsync();

            if (existing == null)
            {
                _context.Settings.Add(settings);
            }
            else
            {
                existing.FestivalName = settings.FestivalName;
                existing.LogoPath = settings.LogoPath;
                existing.PrimaryColor = settings.PrimaryColor;
                existing.SecondaryColor = settings.SecondaryColor;
                existing.AccentColor = settings.AccentColor;
                existing.Tagline = settings.Tagline;
                existing.UpcomingEventYear = settings.UpcomingEventYear;
                existing.UpdatedAt = DateTime.UtcNow;
                existing.UpdatedBy = settings.UpdatedBy;
            }

            await _context.SaveChangesAsync();

            _cachedSettings = existing ?? settings;
            _cacheExpiration = DateTime.UtcNow.AddMinutes(5);

            return existing ?? settings;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<string> SaveLogoAsync(Stream fileStream, string fileName)
    {
        var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "branding");

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var fileStreamOutput = new FileStream(filePath, FileMode.Create))
        {
            await fileStream.CopyToAsync(fileStreamOutput);
        }

        return $"/uploads/branding/{uniqueFileName}";
    }

    public async Task<ApplicationUser?> GetApplicationUserByEmailAsync(string email)
    {
        return await _context.Users
            .Include(u => u.Location)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<Location?> GetLocationByIdAsync(int locationId)
    {
        return await _context.Locations
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == locationId);
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _context.Products
            .Include(p => p.Supplier)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Location>> GetAllLocationsAsync()
    {
        return await _context.Locations
            .AsNoTracking()
            .ToListAsync();
    }
}
