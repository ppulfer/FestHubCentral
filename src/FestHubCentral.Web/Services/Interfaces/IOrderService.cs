using FestHubCentral.Web.Data.Models;

namespace FestHubCentral.Web.Services.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task<IEnumerable<Order>> GetOrdersByLocationAsync(int locationId);
    Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<Order?> GetOrderByIdAsync(int id);
    Task<Order> CreateOrderAsync(Order order);
    Task<decimal> GetTotalSalesByDateAsync(DateTime date);
    Task<decimal> GetTotalSalesByLocationAsync(int locationId, DateTime date);
    Task<Dictionary<string, decimal>> GetSalesByPaymentMethodAsync(DateTime date);
    Task<IEnumerable<object>> GetTopSellingProductsAsync(int count, DateTime? startDate = null);
}
