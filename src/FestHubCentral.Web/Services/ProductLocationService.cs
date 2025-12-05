using FestHubCentral.Web.Data;
using FestHubCentral.Web.Data.Models;
using FestHubCentral.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FestHubCentral.Web.Services;

public class ProductLocationService : IProductLocationService
{
    private readonly ApplicationDbContext _context;

    public ProductLocationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductLocation>> GetAllProductLocationsAsync()
    {
        return await _context.ProductLocations
            .Include(f => f.Product)
                .ThenInclude(p => p.Supplier)
            .Include(f => f.Location)
            .Include(f => f.Event)
            .OrderBy(f => f.Location.Name)
            .ThenBy(f => f.Product.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProductLocation>> GetProductLocationsByEventYearAsync(int eventYear)
    {
        return await _context.ProductLocations
            .Include(f => f.Product)
                .ThenInclude(p => p.Supplier)
            .Include(f => f.Location)
            .Where(f => f.EventYear == eventYear)
            .OrderBy(f => f.Location.Name)
            .ThenBy(f => f.Product.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProductLocation>> GetProductLocationsByLocationAsync(int locationId, int eventYear)
    {
        return await _context.ProductLocations
            .Include(f => f.Product)
                .ThenInclude(p => p.Supplier)
            .Include(f => f.Product)
                .ThenInclude(p => p.ProductEventPrices)
            .Include(f => f.Location)
            .Where(f => f.LocationId == locationId && f.EventYear == eventYear)
            .OrderBy(f => f.Product.Name)
            .ToListAsync();
    }

    public async Task<ProductLocation?> GetProductLocationByIdAsync(int id)
    {
        return await _context.ProductLocations
            .Include(f => f.Product)
            .Include(f => f.Location)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<ProductLocation?> GetProductLocationAsync(int productId, int locationId, int eventYear)
    {
        return await _context.ProductLocations
            .Include(f => f.Product)
            .Include(f => f.Location)
            .FirstOrDefaultAsync(f => f.ProductId == productId
                && f.LocationId == locationId
                && f.EventYear == eventYear);
    }

    public async Task<ProductLocation> CreateProductLocationAsync(ProductLocation productLocation)
    {
        productLocation.CreatedAt = DateTime.UtcNow;
        _context.ProductLocations.Add(productLocation);
        await _context.SaveChangesAsync();
        return productLocation;
    }

    public async Task<ProductLocation> UpdateProductLocationAsync(ProductLocation productLocation)
    {
        productLocation.UpdatedAt = DateTime.UtcNow;
        _context.ProductLocations.Update(productLocation);
        await _context.SaveChangesAsync();
        return productLocation;
    }

    public async Task<bool> DeleteProductLocationAsync(int id)
    {
        var productLocation = await _context.ProductLocations.FindAsync(id);
        if (productLocation == null)
            return false;

        _context.ProductLocations.Remove(productLocation);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ProductLocationExistsAsync(int productId, int locationId, int eventYear)
    {
        return await _context.ProductLocations
            .AnyAsync(f => f.ProductId == productId
                && f.LocationId == locationId
                && f.EventYear == eventYear);
    }
}
