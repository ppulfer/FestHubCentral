using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using FestHubCentral.Web.Data.Models;
using FestHubCentral.Web.Services.Interfaces;

namespace FestHubCentral.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QRCodeController : ControllerBase
{
    private readonly IQRCodeService _qrCodeService;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public QRCodeController(IQRCodeService qrCodeService, SignInManager<ApplicationUser> signInManager)
    {
        _qrCodeService = qrCodeService;
        _signInManager = signInManager;
    }

    [HttpGet("authenticate/{code}")]
    public async Task<IActionResult> AuthenticateWithQRCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return BadRequest("QR code is required");

        var user = await _qrCodeService.AuthenticateWithQRCodeAsync(code);

        if (user == null)
            return Redirect("/account/login?error=invalid-qr");

        await _signInManager.SignInAsync(user, isPersistent: false);

        return Redirect("/");
    }

    [HttpGet("location/{locationId}")]
    public async Task<IActionResult> GetQRCodeForLocation(int locationId)
    {
        var qrCode = await _qrCodeService.GetQRCodeByLocationIdAsync(locationId);

        if (qrCode == null)
            return NotFound(new { error = "QR code not found for this location" });

        return Ok(new
        {
            id = qrCode.Id,
            code = qrCode.Code,
            imageData = qrCode.ImageData,
            scanCount = qrCode.ScanCount,
            lastScannedAt = qrCode.LastScannedAt
        });
    }

    [HttpPost("generate/{locationId}")]
    public async Task<IActionResult> GenerateQRCode(int locationId)
    {
        try
        {
            var qrCode = await _qrCodeService.GenerateQRCodeForLocationAsync(locationId);

            return Ok(new
            {
                id = qrCode.Id,
                code = qrCode.Code,
                imageData = qrCode.ImageData,
                createdAt = qrCode.CreatedAt
            });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
}
