using System.ComponentModel.DataAnnotations;

namespace FestHubCentral.Web.Data.Models;

public class Inventory
{
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int? LocationId { get; set; }
    public Location? Location { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int CurrentStock { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int MinimumStock { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int MaximumStock { get; set; }

    [Range(0, int.MaxValue)]
    public int ReorderQuantity { get; set; }

    [Required]
    public int EventYear { get; set; }
    public Event Event { get; set; } = null!;

    public DateTime LastRestocked { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
