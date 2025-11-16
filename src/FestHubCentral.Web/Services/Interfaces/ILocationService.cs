using FestHubCentral.Web.Data.Models;

namespace FestHubCentral.Web.Services.Interfaces;

public interface ILocationService
{
    Task<IEnumerable<Location>> GetAllLocationsAsync();
    Task<Location?> GetLocationByIdAsync(int id);
    Task<Location> CreateLocationAsync(Location location);
    Task<Location> UpdateLocationAsync(Location location);
    Task DeleteLocationAsync(int id);
}
