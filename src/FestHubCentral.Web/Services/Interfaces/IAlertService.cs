using FestHubCentral.Web.Data.Models;

namespace FestHubCentral.Web.Services.Interfaces;

public interface IAlertService
{
    Task<IEnumerable<Alert>> GetAllAlertsAsync();
    Task<IEnumerable<Alert>> GetUnresolvedAlertsAsync();
    Task<IEnumerable<Alert>> GetAlertsByVendorAsync(int vendorId);
    Task<Alert?> GetAlertByIdAsync(int id);
    Task<Alert> CreateAlertAsync(Alert alert);
    Task<Alert> ResolveAlertAsync(int id, string resolvedBy);
    Task ResolveAlertsByProductAsync(int productId);
    Task<int> GetUnresolvedAlertCountAsync();
}
