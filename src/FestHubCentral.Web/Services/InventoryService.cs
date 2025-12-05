using FestHubCentral.Web.Data;
using FestHubCentral.Web.Data.Models;
using FestHubCentral.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FestHubCentral.Web.Services;

public class InventoryService : IInventoryService
{
    private readonly ApplicationDbContext _context;
    private readonly ISettingsService _settingsService;
    private readonly IProductService _productService;

    public InventoryService(ApplicationDbContext context, ISettingsService settingsService, IProductService productService)
    {
        _context = context;
        _settingsService = settingsService;
        _productService = productService;
    }

    public async Task<IEnumerable<Inventory>> GetAllInventoryAsync()
    {
        var settings = await _settingsService.GetSettingsAsync();
        return await _context.Inventories
            .Include(i => i.Product)
                .ThenInclude(p => p.Supplier)
            .Where(i => i.EventYear == settings.CurrentEventYear)
            .ToListAsync();
    }

    public async Task<IEnumerable<Inventory>> GetInventoryByYearAsync(int year)
    {
        return await _context.Inventories
            .Include(i => i.Product)
                .ThenInclude(p => p.Supplier)
            .Where(i => i.EventYear == year)
            .ToListAsync();
    }

    public async Task<Inventory?> GetInventoryByProductIdAsync(int productId)
    {
        var settings = await _settingsService.GetSettingsAsync();
        return await _context.Inventories
            .Include(i => i.Product)
            .Where(i => i.EventYear == settings.CurrentEventYear)
            .FirstOrDefaultAsync(i => i.ProductId == productId);
    }

    public async Task<Inventory> CreateInventoryAsync(Inventory inventory)
    {
        var settings = await _settingsService.GetSettingsAsync();
        inventory.EventYear = settings.CurrentEventYear;
        inventory.CreatedAt = DateTime.UtcNow;
        _context.Inventories.Add(inventory);
        await _context.SaveChangesAsync();
        return inventory;
    }

    public async Task<Inventory> CreateInventoryWithProductAsync(Product product, int eventYear)
    {
        var createdProduct = await _productService.CreateProductAsync(product);

        var inventory = new Inventory
        {
            ProductId = createdProduct.Id,
            EventYear = eventYear,
            CreatedAt = DateTime.UtcNow
        };

        _context.Inventories.Add(inventory);
        await _context.SaveChangesAsync();
        return inventory;
    }

    public async Task<Inventory> UpdateInventoryAsync(Inventory inventory)
    {
        inventory.UpdatedAt = DateTime.UtcNow;
        _context.Inventories.Update(inventory);
        await _context.SaveChangesAsync();
        return inventory;
    }

    public async Task DeleteInventoryAsync(int id)
    {
        var inventory = await _context.Inventories.FindAsync(id);
        if (inventory != null)
        {
            var productId = inventory.ProductId;
            _context.Inventories.Remove(inventory);
            await _context.SaveChangesAsync();

            if (await _productService.CanDeleteProductAsync(productId))
            {
                await _productService.DeleteProductAsync(productId);
            }
        }
    }
}
