using FestHubCentral.Web.Data;
using FestHubCentral.Web.Data.Models;
using FestHubCentral.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FestHubCentral.Web.Services;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;
    private readonly IInventoryService _inventoryService;
    private readonly ISettingsService _settingsService;

    public OrderService(ApplicationDbContext context, IInventoryService inventoryService, ISettingsService settingsService)
    {
        _context = context;
        _inventoryService = inventoryService;
        _settingsService = settingsService;
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        var settings = await _settingsService.GetSettingsAsync();
        return await _context.Orders
            .Include(o => o.Location)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Where(o => o.EventYear == settings.UpcomingEventYear)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetOrdersByLocationAsync(int locationId)
    {
        var settings = await _settingsService.GetSettingsAsync();
        return await _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Where(o => o.LocationId == locationId && o.EventYear == settings.UpcomingEventYear)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var settings = await _settingsService.GetSettingsAsync();
        return await _context.Orders
            .Include(o => o.Location)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate && o.EventYear == settings.UpcomingEventYear)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        var settings = await _settingsService.GetSettingsAsync();
        return await _context.Orders
            .Include(o => o.Location)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Where(o => o.EventYear == settings.UpcomingEventYear)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var settings = await _settingsService.GetSettingsAsync();
            order.EventYear = settings.UpcomingEventYear;
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
        var settings = await _settingsService.GetSettingsAsync();
        var startDate = DateTime.SpecifyKind(date.Date, DateTimeKind.Utc);
        var endDate = startDate.AddDays(1);
        return await _context.Orders
            .Where(o => o.OrderDate >= startDate && o.OrderDate < endDate && o.EventYear == settings.UpcomingEventYear)
            .SumAsync(o => o.TotalAmount);
    }

    public async Task<decimal> GetTotalSalesByLocationAsync(int locationId, DateTime date)
    {
        var settings = await _settingsService.GetSettingsAsync();
        var startDate = DateTime.SpecifyKind(date.Date, DateTimeKind.Utc);
        var endDate = startDate.AddDays(1);
        return await _context.Orders
            .Where(o => o.LocationId == locationId && o.OrderDate >= startDate && o.OrderDate < endDate && o.EventYear == settings.UpcomingEventYear)
            .SumAsync(o => o.TotalAmount);
    }

    public async Task<Dictionary<string, decimal>> GetSalesByPaymentMethodAsync(DateTime date)
    {
        var settings = await _settingsService.GetSettingsAsync();
        var startDate = DateTime.SpecifyKind(date.Date, DateTimeKind.Utc);
        var endDate = startDate.AddDays(1);
        return await _context.Orders
            .Where(o => o.OrderDate >= startDate && o.OrderDate < endDate && o.EventYear == settings.UpcomingEventYear)
            .GroupBy(o => o.PaymentMethod)
            .Select(g => new { PaymentMethod = g.Key, Total = g.Sum(o => o.TotalAmount) })
            .ToDictionaryAsync(x => x.PaymentMethod, x => x.Total);
    }

    public async Task<IEnumerable<object>> GetTopSellingProductsAsync(int count, DateTime? startDate = null)
    {
        var settings = await _settingsService.GetSettingsAsync();
        var query = _context.OrderItems
            .Include(oi => oi.Product)
            .Where(oi => oi.Order.EventYear == settings.UpcomingEventYear)
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
        var settings = await _settingsService.GetSettingsAsync();
        var today = DateTime.UtcNow;
        var prefix = $"ORD-{today:yyyyMMdd}";
        var count = await _context.Orders
            .Where(o => o.EventYear == settings.UpcomingEventYear)
            .CountAsync(o => o.OrderNumber.StartsWith(prefix));
        return $"{prefix}-{(count + 1):D4}";
    }
}
