using FestHubCentral.Web.Data.Models;

namespace FestHubCentral.Web.Services.Interfaces;

public interface ISettingsService
{
    Task<Settings> GetSettingsAsync();
    Task<Settings> UpdateSettingsAsync(Settings settings);
    Task<string> SaveLogoAsync(Stream fileStream, string fileName);
    Task<ApplicationUser?> GetApplicationUserByEmailAsync(string email);
    Task<Location?> GetLocationByIdAsync(int locationId);
    Task<List<Product>> GetAllProductsAsync();
    Task<List<Location>> GetAllLocationsAsync();
}
