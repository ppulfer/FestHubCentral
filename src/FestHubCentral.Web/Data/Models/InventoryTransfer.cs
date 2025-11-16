using System.ComponentModel.DataAnnotations;

namespace FestHubCentral.Web.Data.Models;

public class InventoryTransfer
{
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int? FromLocationId { get; set; }
    public Location? FromLocation { get; set; }

    public int? ToLocationId { get; set; }
    public Location? ToLocation { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Amount { get; set; }

    [MaxLength(500)]
    public string? Comment { get; set; }

    [Required]
    public int EventYear { get; set; }
    public Event Event { get; set; } = null!;

    public DateTime TransferDate { get; set; }

    public string? CreatedByUserId { get; set; }
    public ApplicationUser? CreatedByUser { get; set; }

    public DateTime CreatedAt { get; set; }
}
