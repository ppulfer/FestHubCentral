using FestHubCentral.Web.Data;
using FestHubCentral.Web.Data.Models;
using FestHubCentral.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FestHubCentral.Web.Services;

public class BrandingService : IBrandingService
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public BrandingService(ApplicationDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public async Task<BrandingSettings> GetBrandingSettingsAsync()
    {
        var settings = await _context.BrandingSettings.FirstOrDefaultAsync();

        if (settings == null)
        {
            settings = new BrandingSettings
            {
                FestivalName = "FestHub Central",
                PrimaryColor = "#6366f1",
                SecondaryColor = "#8b5cf6",
                AccentColor = "#ec4899",
                Tagline = "Real-time festival gastronomy management",
                UpdatedAt = DateTime.UtcNow
            };

            _context.BrandingSettings.Add(settings);
            await _context.SaveChangesAsync();
        }

        return settings;
    }

    public async Task<BrandingSettings> UpdateBrandingSettingsAsync(BrandingSettings settings)
    {
        var existing = await _context.BrandingSettings.FirstOrDefaultAsync();

        if (existing == null)
        {
            _context.BrandingSettings.Add(settings);
        }
        else
        {
            existing.FestivalName = settings.FestivalName;
            existing.LogoPath = settings.LogoPath;
            existing.PrimaryColor = settings.PrimaryColor;
            existing.SecondaryColor = settings.SecondaryColor;
            existing.AccentColor = settings.AccentColor;
            existing.Tagline = settings.Tagline;
            existing.UpdatedAt = DateTime.UtcNow;
            existing.UpdatedBy = settings.UpdatedBy;
        }

        await _context.SaveChangesAsync();
        return existing ?? settings;
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
}
