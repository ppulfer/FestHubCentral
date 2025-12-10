using FestHubCentral.Web.Data.Models;

namespace FestHubCentral.Web.Services.Interfaces;

public interface IQRCodeService
{
    Task<QRCode> GenerateQRCodeForLocationAsync(int locationId);
    Task<QRCode?> GetQRCodeByLocationIdAsync(int locationId);
    Task<ApplicationUser?> AuthenticateWithQRCodeAsync(string code);
}
