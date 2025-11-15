using FestHubCentral.Web.Data.Models;

namespace FestHubCentral.Web.Services.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task<IEnumerable<Order>> GetOrdersByVendorAsync(int vendorId);
    Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<Order?> GetOrderByIdAsync(int id);
    Task<Order> CreateOrderAsync(Order order);
    Task<decimal> GetTotalSalesByDateAsync(DateTime date);
    Task<decimal> GetTotalSalesByVendorAsync(int vendorId, DateTime date);
    Task<Dictionary<string, decimal>> GetSalesByPaymentMethodAsync(DateTime date);
    Task<IEnumerable<object>> GetTopSellingProductsAsync(int count, DateTime? startDate = null);
}
