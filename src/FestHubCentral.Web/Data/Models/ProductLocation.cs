using System.ComponentModel.DataAnnotations;

namespace FestHubCentral.Web.Data.Models;

public class ProductLocation
{
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    [Required]
    public int LocationId { get; set; }
    public Location Location { get; set; } = null!;

    [Required]
    [Range(0, int.MaxValue)]
    public int PlannedAmount { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int InitialDelivery { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    [Required]
    public int EventYear { get; set; }
    public Event Event { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
