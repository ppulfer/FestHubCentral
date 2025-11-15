using FestHubCentral.Web.Data;
using FestHubCentral.Web.Data.Models;
using FestHubCentral.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FestHubCentral.Web.Services;

public class InventoryService : IInventoryService
{
    private readonly ApplicationDbContext _context;
    private readonly IAlertService _alertService;

    public InventoryService(ApplicationDbContext context, IAlertService alertService)
    {
        _context = context;
        _alertService = alertService;
    }

    public async Task<IEnumerable<Inventory>> GetAllInventoryAsync()
    {
        return await _context.Inventories
            .Include(i => i.Product)
                .ThenInclude(p => p.Vendor)
            .ToListAsync();
    }

    public async Task<IEnumerable<Inventory>> GetLowStockItemsAsync()
    {
        return await _context.Inventories
            .Include(i => i.Product)
                .ThenInclude(p => p.Vendor)
            .Where(i => i.CurrentStock <= i.MinimumStock)
            .ToListAsync();
    }

    public async Task<Inventory?> GetInventoryByProductIdAsync(int productId)
    {
        return await _context.Inventories
            .Include(i => i.Product)
            .FirstOrDefaultAsync(i => i.ProductId == productId);
    }

    public async Task<Inventory> CreateInventoryAsync(Inventory inventory)
    {
        inventory.CreatedAt = DateTime.UtcNow;
        inventory.LastRestocked = DateTime.UtcNow;
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

    public async Task RestockAsync(int productId, int quantity)
    {
        var inventory = await GetInventoryByProductIdAsync(productId);
        if (inventory != null)
        {
            inventory.CurrentStock += quantity;
            inventory.LastRestocked = DateTime.UtcNow;
            inventory.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var product = await _context.Products.FindAsync(productId);
            if (product != null && inventory.CurrentStock > inventory.MinimumStock)
            {
                await _alertService.ResolveAlertsByProductAsync(productId);
            }
        }
    }

    public async Task DecrementStockAsync(int productId, int quantity)
    {
        var inventory = await GetInventoryByProductIdAsync(productId);
        if (inventory != null)
        {
            inventory.CurrentStock -= quantity;
            inventory.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            if (inventory.CurrentStock <= inventory.MinimumStock)
            {
                var product = await _context.Products
                    .Include(p => p.Vendor)
                    .FirstOrDefaultAsync(p => p.Id == productId);

                if (product != null)
                {
                    var severity = inventory.CurrentStock <= (inventory.MinimumStock / 2)
                        ? "Critical"
                        : "Warning";

                    await _alertService.CreateAlertAsync(new Alert
                    {
                        Type = "LowInventory",
                        Severity = severity,
                        Title = "Low Stock Alert",
                        Message = $"{product.Name} at {product.Vendor.Name} is running low. Current stock: {inventory.CurrentStock}, Minimum: {inventory.MinimumStock}",
                        VendorId = product.VendorId,
                        ProductId = productId
                    });
                }
            }
        }
    }

    public async Task<bool> CheckStockAvailabilityAsync(int productId, int quantity)
    {
        var inventory = await GetInventoryByProductIdAsync(productId);
        return inventory != null && inventory.CurrentStock >= quantity;
    }
}
