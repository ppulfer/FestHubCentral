using FestHubCentral.Web.Data.Models;

namespace FestHubCentral.Web.Services.Interfaces;

public interface IInventoryService
{
    Task<IEnumerable<Inventory>> GetAllInventoryAsync();
    Task<IEnumerable<Inventory>> GetLowStockItemsAsync();
    Task<Inventory?> GetInventoryByProductIdAsync(int productId);
    Task<Inventory> CreateInventoryAsync(Inventory inventory);
    Task<Inventory> UpdateInventoryAsync(Inventory inventory);
    Task RestockAsync(int productId, int quantity);
    Task DecrementStockAsync(int productId, int quantity);
    Task<bool> CheckStockAvailabilityAsync(int productId, int quantity);
}
