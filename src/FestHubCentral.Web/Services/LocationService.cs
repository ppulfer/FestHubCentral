using FestHubCentral.Web.Data;
using FestHubCentral.Web.Data.Models;
using FestHubCentral.Web.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FestHubCentral.Web.Services;

public class LocationService : ILocationService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public LocationService(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IEnumerable<Location>> GetAllLocationsAsync()
    {
        return await _context.Locations
            .OrderBy(v => v.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Location>> GetLocationsByEventYearAsync(int eventYear)
    {
        return await _context.EventLocations
            .Where(el => el.EventYear == eventYear && el.IsActive)
            .Include(el => el.Location)
            .Select(el => el.Location)
            .OrderBy(l => l.Name)
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

        if (!await _roleManager.RoleExistsAsync("Location"))
        {
            await _roleManager.CreateAsync(new IdentityRole("Location"));
        }

        var username = "location" + location.Id;
        if (await _userManager.FindByNameAsync(username) == null)
        {
            var locationUser = new ApplicationUser
            {
                UserName = username,
                Email = username + "@festhub.ch",
                DisplayName = location.Name,
                EmailConfirmed = true,
                RequiresPasswordChange = false,
                LocationId = location.Id,
            };

            var result = await _userManager.CreateAsync(locationUser, "Start1234!");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(locationUser, "Location");
            }
        }

        return location;
    }

    public async Task<Location> UpdateLocationAsync(Location location)
    {
        var existingLocation = await _context.Locations.FindAsync(location.Id);
        if (existingLocation == null)
        {
            throw new InvalidOperationException($"Location with ID {location.Id} not found.");
        }

        existingLocation.Name = location.Name;
        existingLocation.Category = location.Category;
        existingLocation.Latitude = location.Latitude;
        existingLocation.Longitude = location.Longitude;
        existingLocation.ContactPerson = location.ContactPerson;
        existingLocation.ContactPhone = location.ContactPhone;
        existingLocation.ContactEmail = location.ContactEmail;
        existingLocation.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existingLocation;
    }

    public async Task DeleteLocationAsync(int id)
    {
        var location = await _context.Locations.FindAsync(id);
        if (location != null)
        {
            var locationUsers = await _context.Users
                .Where(u => u.LocationId == id)
                .ToListAsync();

            _context.Users.RemoveRange(locationUsers);

            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<EventLocation> AddLocationToEventAsync(int locationId, int eventYear)
    {
        var existingEventLocation = await _context.EventLocations
            .FirstOrDefaultAsync(el => el.LocationId == locationId && el.EventYear == eventYear);

        if (existingEventLocation != null)
        {
            existingEventLocation.IsActive = true;
            existingEventLocation.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return existingEventLocation;
        }

        var eventLocation = new EventLocation
        {
            LocationId = locationId,
            EventYear = eventYear,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.EventLocations.Add(eventLocation);
        await _context.SaveChangesAsync();
        return eventLocation;
    }

    public async Task RemoveLocationFromEventAsync(int locationId, int eventYear)
    {
        var eventLocation = await _context.EventLocations
            .FirstOrDefaultAsync(el => el.LocationId == locationId && el.EventYear == eventYear);

        if (eventLocation != null)
        {
            _context.EventLocations.Remove(eventLocation);
            await _context.SaveChangesAsync();

            if (await CanDeleteLocationAsync(locationId))
            {
                await DeleteLocationAsync(locationId);
            }
        }
    }

    public async Task<bool> CanDeleteLocationAsync(int locationId)
    {
        var hasEventLocations = await _context.EventLocations.AnyAsync(el => el.LocationId == locationId);
        var hasProductLocations = await _context.ProductLocations.AnyAsync(pl => pl.LocationId == locationId);
        var hasInventoryTransfersFrom = await _context.InventoryTransfers.AnyAsync(it => it.FromLocationId == locationId);
        var hasInventoryTransfersTo = await _context.InventoryTransfers.AnyAsync(it => it.ToLocationId == locationId);
        var hasTransferRequestsFrom = await _context.TransferRequests.AnyAsync(tr => tr.FromLocationId == locationId);
        var hasTransferRequestsTo = await _context.TransferRequests.AnyAsync(tr => tr.ToLocationId == locationId);
        var hasCashRegisters = await _context.CashRegisters.AnyAsync(cr => cr.LocationId == locationId);
        
        return !hasEventLocations && !hasProductLocations && !hasInventoryTransfersFrom &&
               !hasInventoryTransfersTo && !hasTransferRequestsFrom && !hasTransferRequestsTo &&
               !hasCashRegisters;
    }

}
