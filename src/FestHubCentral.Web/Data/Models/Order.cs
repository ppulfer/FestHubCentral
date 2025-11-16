using System.ComponentModel.DataAnnotations;

namespace FestHubCentral.Web.Data.Models;

public class Order
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string OrderNumber { get; set; } = string.Empty;

    [Required]
    public int LocationId { get; set; }
    public Location Location { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string PaymentMethod { get; set; } = string.Empty;

    [Required]
    [Range(0, 1000000)]
    public decimal TotalAmount { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    public DateTime OrderDate { get; set; }

    [Required]
    public int EventYear { get; set; }
    public Event Event { get; set; } = null!;

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
