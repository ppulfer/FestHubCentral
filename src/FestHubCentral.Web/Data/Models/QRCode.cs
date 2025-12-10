namespace FestHubCentral.Web.Data.Models;

public class QRCode
{
    public int Id { get; set; }
    public int LocationId { get; set; }
    public Location? Location { get; set; }
    public string Code { get; set; } = string.Empty;
    public string? ImageData { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastScannedAt { get; set; }
    public int ScanCount { get; set; }
}
