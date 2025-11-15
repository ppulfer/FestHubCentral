using FestHubCentral.Web.Data;
using FestHubCentral.Web.Data.Models;
using FestHubCentral.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FestHubCentral.Web.Services;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;
    private readonly IInventoryService _inventoryService;

    public OrderService(ApplicationDbContext context, IInventoryService inventoryService)
    {
        _context = context;
        _inventoryService = inventoryService;
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders
            .Include(o => o.Vendor)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetOrdersByVendorAsync(int vendorId)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Where(o => o.VendorId == vendorId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Orders
            .Include(o => o.Vendor)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.Vendor)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            order.OrderNumber = await GenerateOrderNumberAsync();
            order.OrderDate = DateTime.UtcNow;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in order.OrderItems)
            {
                await _inventoryService.DecrementStockAsync(item.ProductId, item.Quantity);
            }

            await transaction.CommitAsync();
            return order;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<decimal> GetTotalSalesByDateAsync(DateTime date)
    {
        return await _context.Orders
            .Where(o => o.OrderDate.Date == date.Date)
            .SumAsync(o => o.TotalAmount);
    }

    public async Task<decimal> GetTotalSalesByVendorAsync(int vendorId, DateTime date)
    {
        return await _context.Orders
            .Where(o => o.VendorId == vendorId && o.OrderDate.Date == date.Date)
            .SumAsync(o => o.TotalAmount);
    }

    public async Task<Dictionary<string, decimal>> GetSalesByPaymentMethodAsync(DateTime date)
    {
        return await _context.Orders
            .Where(o => o.OrderDate.Date == date.Date)
            .GroupBy(o => o.PaymentMethod)
            .Select(g => new { PaymentMethod = g.Key, Total = g.Sum(o => o.TotalAmount) })
            .ToDictionaryAsync(x => x.PaymentMethod, x => x.Total);
    }

    public async Task<IEnumerable<object>> GetTopSellingProductsAsync(int count, DateTime? startDate = null)
    {
        var query = _context.OrderItems
            .Include(oi => oi.Product)
            .AsQueryable();

        if (startDate.HasValue)
        {
            query = query.Where(oi => oi.Order.OrderDate >= startDate.Value);
        }

        return await query
            .GroupBy(oi => new { oi.ProductId, oi.Product.Name })
            .Select(g => new
            {
                ProductId = g.Key.ProductId,
                ProductName = g.Key.Name,
                TotalQuantity = g.Sum(oi => oi.Quantity),
                TotalRevenue = g.Sum(oi => oi.Subtotal)
            })
            .OrderByDescending(x => x.TotalQuantity)
            .Take(count)
            .ToListAsync<object>();
    }

    private async Task<string> GenerateOrderNumberAsync()
    {
        var today = DateTime.UtcNow;
        var prefix = $"ORD-{today:yyyyMMdd}";
        var count = await _context.Orders
            .CountAsync(o => o.OrderNumber.StartsWith(prefix));
        return $"{prefix}-{(count + 1):D4}";
    }
}
