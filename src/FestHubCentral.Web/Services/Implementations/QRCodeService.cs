using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using QRCoder;
using FestHubCentral.Web.Data;
using FestHubCentral.Web.Data.Models;
using FestHubCentral.Web.Services.Interfaces;

namespace FestHubCentral.Web.Services.Implementations;

public class QRCodeService : IQRCodeService
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public QRCodeService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<QRCode> GenerateQRCodeForLocationAsync(int locationId)
    {
        var location = await _context.Locations.FindAsync(locationId);
        if (location == null)
            throw new InvalidOperationException($"Location with ID {locationId} not found");

        // Check if QR code already exists
        var existingQR = await _context.QRCodes.FirstOrDefaultAsync(q => q.LocationId == locationId);
        if (existingQR != null)
            return existingQR;

        // Generate unique code
        var code = GenerateUniqueCode(locationId);

        // Generate QR code image with full URL
        var baseUrl = GetBaseUrl();
        var qrUrl = $"{baseUrl}/api/qrcode/authenticate/{code}";
        var imageData = GenerateQRImage(qrUrl);

        var qrCode = new QRCode
        {
            LocationId = locationId,
            Code = code,
            ImageData = imageData,
            CreatedAt = DateTime.UtcNow
        };

        _context.QRCodes.Add(qrCode);
        await _context.SaveChangesAsync();

        return qrCode;
    }

    public async Task<QRCode?> GetQRCodeByLocationIdAsync(int locationId)
    {
        return await _context.QRCodes.FirstOrDefaultAsync(q => q.LocationId == locationId);
    }

    public async Task<QRCode?> GetQRCodeByCodeAsync(string code)
    {
        return await _context.QRCodes.FirstOrDefaultAsync(q => q.Code == code);
    }

    public async Task<ApplicationUser?> AuthenticateWithQRCodeAsync(string code)
    {
        var qrCode = await GetQRCodeByCodeAsync(code);
        if (qrCode == null)
            return null;

        var locationUser = await _context.Users
            .FirstOrDefaultAsync(u => u.LocationId == qrCode.LocationId);

        if (locationUser != null)
        {
            await RecordQRCodeScanAsync(qrCode.Id);
        }

        return locationUser;
    }

    public async Task RecordQRCodeScanAsync(int qrCodeId)
    {
        var qrCode = await _context.QRCodes.FindAsync(qrCodeId);
        if (qrCode != null)
        {
            qrCode.LastScannedAt = DateTime.UtcNow;
            qrCode.ScanCount++;
            await _context.SaveChangesAsync();
        }
    }

    private string GenerateUniqueCode(int locationId)
    {
        // Format: LOC-{locationId}-{timestamp}
        var timestamp = DateTime.UtcNow.Ticks;
        return $"LOC-{locationId}-{timestamp}";
    }

    private string GenerateQRImage(string data)
    {
        using (var qrGenerator = new QRCodeGenerator())
        {
            var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            using (var qrCode = new PngByteQRCode(qrCodeData))
            {
                var qrCodeImage = qrCode.GetGraphic(10);
                return "data:image/png;base64," + Convert.ToBase64String(qrCodeImage);
            }
        }
    }

    private string GetBaseUrl()
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        if (request == null)
            return "https://localhost:5001";

        return $"{request.Scheme}://{request.Host}";
    }
}
