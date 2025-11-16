using FestHubCentral.Web.Data.Models;

namespace FestHubCentral.Web.Services.Interfaces;

public interface ISettingsService
{
    Task<Settings> GetSettingsAsync();
    Task<Settings> UpdateSettingsAsync(Settings settings);
    Task<string> SaveLogoAsync(Stream fileStream, string fileName);
}
