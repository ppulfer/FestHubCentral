using System.ComponentModel.DataAnnotations;

namespace FestHubCentral.Web.Data.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    [MaxLength(50)]
    public string Unit { get; set; } = string.Empty;

    public bool IsAvailable { get; set; } = true;

    [Required]
    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; } = null!;

    public Inventory? Inventory { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public ICollection<ProductEventPrice> ProductEventPrices { get; set; } = new List<ProductEventPrice>();

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
