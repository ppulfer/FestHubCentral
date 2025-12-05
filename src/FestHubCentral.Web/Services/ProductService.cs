using FestHubCentral.Web.Data;
using FestHubCentral.Web.Data.Models;
using FestHubCentral.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FestHubCentral.Web.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _context.Products
            .Include(p => p.Supplier)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Supplier)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        product.CreatedAt = DateTime.UtcNow;
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        product.UpdatedAt = DateTime.UtcNow;
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<bool> CanDeleteProductAsync(int productId)
    {
        var hasInventories = await _context.Inventories.AnyAsync(i => i.ProductId == productId);
        var hasProductEventPrices = await _context.ProductEventPrices.AnyAsync(pep => pep.ProductId == productId);
        var hasProductLocations = await _context.ProductLocations.AnyAsync(pl => pl.ProductId == productId);
        var hasInventoryTransfers = await _context.InventoryTransfers.AnyAsync(it => it.ProductId == productId);
        var hasTransferRequests = await _context.TransferRequests.AnyAsync(tr => tr.ProductId == productId);

        return !hasInventories && !hasProductEventPrices &&
               !hasProductLocations && !hasInventoryTransfers && !hasTransferRequests;
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
