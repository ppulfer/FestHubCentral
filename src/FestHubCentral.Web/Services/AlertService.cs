using FestHubCentral.Web.Data;
using FestHubCentral.Web.Data.Models;
using FestHubCentral.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FestHubCentral.Web.Services;

public class AlertService : IAlertService
{
    private readonly ApplicationDbContext _context;

    public AlertService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Alert>> GetAllAlertsAsync()
    {
        return await _context.Alerts
            .Include(a => a.Vendor)
            .Include(a => a.Product)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Alert>> GetUnresolvedAlertsAsync()
    {
        return await _context.Alerts
            .Include(a => a.Vendor)
            .Include(a => a.Product)
            .Where(a => !a.IsResolved)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Alert>> GetAlertsByVendorAsync(int vendorId)
    {
        return await _context.Alerts
            .Include(a => a.Product)
            .Where(a => a.VendorId == vendorId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<Alert?> GetAlertByIdAsync(int id)
    {
        return await _context.Alerts
            .Include(a => a.Vendor)
            .Include(a => a.Product)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Alert> CreateAlertAsync(Alert alert)
    {
        alert.CreatedAt = DateTime.UtcNow;
        _context.Alerts.Add(alert);
        await _context.SaveChangesAsync();
        return alert;
    }

    public async Task<Alert> ResolveAlertAsync(int id, string resolvedBy)
    {
        var alert = await _context.Alerts.FindAsync(id);
        if (alert != null)
        {
            alert.IsResolved = true;
            alert.ResolvedAt = DateTime.UtcNow;
            alert.ResolvedBy = resolvedBy;
            await _context.SaveChangesAsync();
        }
        return alert!;
    }

    public async Task ResolveAlertsByProductAsync(int productId)
    {
        var alerts = await _context.Alerts
            .Where(a => a.ProductId == productId && !a.IsResolved)
            .ToListAsync();

        foreach (var alert in alerts)
        {
            alert.IsResolved = true;
            alert.ResolvedAt = DateTime.UtcNow;
            alert.ResolvedBy = "System";
        }

        await _context.SaveChangesAsync();
    }

    public async Task<int> GetUnresolvedAlertCountAsync()
    {
        return await _context.Alerts.CountAsync(a => !a.IsResolved);
    }
}
