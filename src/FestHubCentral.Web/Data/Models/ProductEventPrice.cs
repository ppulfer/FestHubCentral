using System.ComponentModel.DataAnnotations;

namespace FestHubCentral.Web.Data.Models;

public class ProductEventPrice
{
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    [Required]
    public int EventYear { get; set; }
    public Event Event { get; set; } = null!;

    [Required]
    [Range(0.01, 1000000)]
    public decimal PurchasePrice { get; set; }

    [Required]
    [Range(0.01, 1000000)]
    public decimal SellingPrice { get; set; }

    [Range(0, 1000000)]
    public decimal? SpecialPrice { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
