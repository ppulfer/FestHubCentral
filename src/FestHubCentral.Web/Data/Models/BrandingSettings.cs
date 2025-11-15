using System.ComponentModel.DataAnnotations;

namespace FestHubCentral.Web.Data.Models;

public class BrandingSettings
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string FestivalName { get; set; } = "FestHub Central";

    [MaxLength(500)]
    public string? LogoPath { get; set; }

    [MaxLength(50)]
    public string PrimaryColor { get; set; } = "#6366f1";

    [MaxLength(50)]
    public string SecondaryColor { get; set; } = "#8b5cf6";

    [MaxLength(50)]
    public string AccentColor { get; set; } = "#ec4899";

    [MaxLength(200)]
    public string? Tagline { get; set; }

    public DateTime UpdatedAt { get; set; }

    [MaxLength(100)]
    public string? UpdatedBy { get; set; }
}
