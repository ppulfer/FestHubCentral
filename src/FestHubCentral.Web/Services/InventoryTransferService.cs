using FestHubCentral.Web.Data;
using FestHubCentral.Web.Data.Models;
using FestHubCentral.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FestHubCentral.Web.Services;

public class InventoryTransferService : IInventoryTransferService
{
    private readonly ApplicationDbContext _context;
    private readonly ISettingsService _settingsService;

    public InventoryTransferService(ApplicationDbContext context, ISettingsService settingsService)
    {
        _context = context;
        _settingsService = settingsService;
    }

    public async Task<IEnumerable<InventoryTransfer>> GetAllTransfersAsync()
    {
        var settings = await _settingsService.GetSettingsAsync();
        return await _context.InventoryTransfers
            .Include(t => t.Product)
            .Include(t => t.FromLocation)
            .Include(t => t.ToLocation)
            .Include(t => t.CreatedByUser)
            .Where(t => t.EventYear == settings.CurrentEventYear)
            .OrderByDescending(t => t.TransferDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<InventoryTransfer>> GetTransfersByProductAsync(int productId)
    {
        var settings = await _settingsService.GetSettingsAsync();
        return await _context.InventoryTransfers
            .Include(t => t.Product)
            .Include(t => t.FromLocation)
            .Include(t => t.ToLocation)
            .Include(t => t.CreatedByUser)
            .Where(t => t.ProductId == productId && t.EventYear == settings.CurrentEventYear)
            .OrderByDescending(t => t.TransferDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<InventoryTransfer>> GetTransfersByLocationAsync(int locationId)
    {
        var settings = await _settingsService.GetSettingsAsync();
        return await _context.InventoryTransfers
            .Include(t => t.Product)
            .Include(t => t.FromLocation)
            .Include(t => t.ToLocation)
            .Include(t => t.CreatedByUser)
            .Where(t => (t.FromLocationId == locationId || t.ToLocationId == locationId)
                       && t.EventYear == settings.CurrentEventYear)
            .OrderByDescending(t => t.TransferDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<InventoryTransfer>> GetTransfersByLocationAndYearAsync(int locationId, int eventYear)
    {
        return await _context.InventoryTransfers
            .Include(t => t.Product)
            .Include(t => t.FromLocation)
            .Include(t => t.ToLocation)
            .Include(t => t.CreatedByUser)
            .Where(t => (t.FromLocationId == locationId || t.ToLocationId == locationId)
                       && t.EventYear == eventYear)
            .OrderByDescending(t => t.TransferDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<InventoryTransfer>> GetTransfersByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var settings = await _settingsService.GetSettingsAsync();
        var utcStart = DateTime.SpecifyKind(startDate.Date, DateTimeKind.Utc);
        var utcEnd = DateTime.SpecifyKind(endDate.Date.AddDays(1), DateTimeKind.Utc);

        return await _context.InventoryTransfers
            .Include(t => t.Product)
            .Include(t => t.FromLocation)
            .Include(t => t.ToLocation)
            .Include(t => t.CreatedByUser)
            .Where(t => t.TransferDate >= utcStart && t.TransferDate < utcEnd
                       && t.EventYear == settings.CurrentEventYear)
            .OrderByDescending(t => t.TransferDate)
            .ToListAsync();
    }

    public async Task<InventoryTransfer?> GetTransferByIdAsync(int id)
    {
        var settings = await _settingsService.GetSettingsAsync();
        return await _context.InventoryTransfers
            .Include(t => t.Product)
            .Include(t => t.FromLocation)
            .Include(t => t.ToLocation)
            .Include(t => t.CreatedByUser)
            .Where(t => t.EventYear == settings.CurrentEventYear)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<InventoryTransfer> CreateTransferAsync(InventoryTransfer transfer)
    {
        var settings = await _settingsService.GetSettingsAsync();
        transfer.EventYear = settings.CurrentEventYear;
        transfer.TransferDate = DateTime.UtcNow;
        transfer.CreatedAt = DateTime.UtcNow;

        _context.InventoryTransfers.Add(transfer);
        await _context.SaveChangesAsync();

        return transfer;
    }

    public async Task<bool> DeleteTransferAsync(int id)
    {
        var transfer = await _context.InventoryTransfers.FindAsync(id);
        if (transfer == null)
            return false;

        _context.InventoryTransfers.Remove(transfer);
        await _context.SaveChangesAsync();
        return true;
    }
}
