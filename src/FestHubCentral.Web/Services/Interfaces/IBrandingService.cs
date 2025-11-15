using FestHubCentral.Web.Data.Models;

namespace FestHubCentral.Web.Services.Interfaces;

public interface IBrandingService
{
    Task<BrandingSettings> GetBrandingSettingsAsync();
    Task<BrandingSettings> UpdateBrandingSettingsAsync(BrandingSettings settings);
    Task<string> SaveLogoAsync(Stream fileStream, string fileName);
}
