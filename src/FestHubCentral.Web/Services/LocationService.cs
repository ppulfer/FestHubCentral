using FestHubCentral.Web.Data;
using FestHubCentral.Web.Data.Models;
using FestHubCentral.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FestHubCentral.Web.Services;

public class LocationService : ILocationService
{
    private readonly ApplicationDbContext _context;

    public LocationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Location>> GetAllLocationsAsync()
    {
        return await _context.Locations
            .OrderBy(v => v.LocationSpot)
            .ToListAsync();
    }

    public async Task<Location?> GetLocationByIdAsync(int id)
    {
        return await _context.Locations
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<Location> CreateLocationAsync(Location location)
    {
        location.CreatedAt = DateTime.UtcNow;
        _context.Locations.Add(location);
        await _context.SaveChangesAsync();
        return location;
    }

    public async Task<Location> UpdateLocationAsync(Location location)
    {
        location.UpdatedAt = DateTime.UtcNow;
        _context.Locations.Update(location);
        await _context.SaveChangesAsync();
        return location;
    }

    public async Task DeleteLocationAsync(int id)
    {
        var location = await _context.Locations.FindAsync(id);
        if (location != null)
        {
            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();
        }
    }

}
