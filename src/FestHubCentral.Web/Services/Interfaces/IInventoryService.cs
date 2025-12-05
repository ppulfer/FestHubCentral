using FestHubCentral.Web.Data.Models;

namespace FestHubCentral.Web.Services.Interfaces;

public interface IInventoryService
{
    Task<IEnumerable<Inventory>> GetAllInventoryAsync();
    Task<IEnumerable<Inventory>> GetInventoryByYearAsync(int year);
    Task<Inventory?> GetInventoryByProductIdAsync(int productId);
    Task<Inventory> CreateInventoryAsync(Inventory inventory);
    Task<Inventory> CreateInventoryWithProductAsync(Product product, int eventYear);
    Task<Inventory> UpdateInventoryAsync(Inventory inventory);
    Task DeleteInventoryAsync(int id);
}
