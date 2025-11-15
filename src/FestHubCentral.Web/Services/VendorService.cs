using FestHubCentral.Web.Data;
using FestHubCentral.Web.Data.Models;
using FestHubCentral.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FestHubCentral.Web.Services;

public class VendorService : IVendorService
{
    private readonly ApplicationDbContext _context;

    public VendorService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Vendor>> GetAllVendorsAsync()
    {
        return await _context.Vendors
            .Include(v => v.Products)
            .OrderBy(v => v.LocationSpot)
            .ToListAsync();
    }

    public async Task<Vendor?> GetVendorByIdAsync(int id)
    {
        return await _context.Vendors
            .Include(v => v.Products)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<Vendor> CreateVendorAsync(Vendor vendor)
    {
        vendor.CreatedAt = DateTime.UtcNow;
        _context.Vendors.Add(vendor);
        await _context.SaveChangesAsync();
        return vendor;
    }

    public async Task<Vendor> UpdateVendorAsync(Vendor vendor)
    {
        vendor.UpdatedAt = DateTime.UtcNow;
        _context.Vendors.Update(vendor);
        await _context.SaveChangesAsync();
        return vendor;
    }

    public async Task DeleteVendorAsync(int id)
    {
        var vendor = await _context.Vendors.FindAsync(id);
        if (vendor != null)
        {
            _context.Vendors.Remove(vendor);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ToggleVendorStatusAsync(int id)
    {
        var vendor = await _context.Vendors.FindAsync(id);
        if (vendor != null)
        {
            vendor.IsOpen = !vendor.IsOpen;
            vendor.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return vendor.IsOpen;
        }
        return false;
    }

    public async Task<Dictionary<int, bool>> GetVendorStatusMapAsync()
    {
        return await _context.Vendors
            .ToDictionaryAsync(v => v.LocationSpot, v => v.IsOpen);
    }
}
