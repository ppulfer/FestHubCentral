using FestHubCentral.Web.Data.Models;

namespace FestHubCentral.Web.Services.Interfaces;

public interface IInventoryTransferService
{
    Task<IEnumerable<InventoryTransfer>> GetAllTransfersAsync();
    Task<IEnumerable<InventoryTransfer>> GetTransfersByProductAsync(int productId);
    Task<IEnumerable<InventoryTransfer>> GetTransfersByLocationAsync(int locationId);
    Task<IEnumerable<InventoryTransfer>> GetTransfersByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<InventoryTransfer?> GetTransferByIdAsync(int id);
    Task<InventoryTransfer> CreateTransferAsync(InventoryTransfer transfer);
    Task<bool> DeleteTransferAsync(int id);
}
