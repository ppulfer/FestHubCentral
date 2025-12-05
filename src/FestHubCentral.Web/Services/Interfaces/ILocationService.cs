using FestHubCentral.Web.Data.Models;

namespace FestHubCentral.Web.Services.Interfaces;

public interface ILocationService
{
    Task<IEnumerable<Location>> GetAllLocationsAsync();
    Task<IEnumerable<Location>> GetLocationsByEventYearAsync(int eventYear);
    Task<Location?> GetLocationByIdAsync(int id);
    Task<Location> CreateLocationAsync(Location location);
    Task<Location> UpdateLocationAsync(Location location);
    Task DeleteLocationAsync(int id);
    Task<EventLocation> AddLocationToEventAsync(int locationId, int eventYear);
    Task RemoveLocationFromEventAsync(int locationId, int eventYear);
    Task<bool> CanDeleteLocationAsync(int locationId);
}
