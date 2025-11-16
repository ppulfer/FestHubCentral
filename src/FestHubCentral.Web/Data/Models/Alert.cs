using System.ComponentModel.DataAnnotations;

namespace FestHubCentral.Web.Data.Models;

public class Alert
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Type { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Severity { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Message { get; set; } = string.Empty;

    public int? LocationId { get; set; }
    public Location? Location { get; set; }

    public int? ProductId { get; set; }
    public Product? Product { get; set; }

    public bool IsResolved { get; set; }
    public DateTime? ResolvedAt { get; set; }

    [MaxLength(500)]
    public string? ResolvedBy { get; set; }

    public DateTime CreatedAt { get; set; }
}
