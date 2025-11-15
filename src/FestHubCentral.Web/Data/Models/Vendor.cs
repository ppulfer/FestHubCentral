using System.ComponentModel.DataAnnotations;

namespace FestHubCentral.Web.Data.Models;

public class Vendor
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    [Range(1, 15)]
    public int LocationSpot { get; set; }

    [MaxLength(100)]
    public string? ContactPerson { get; set; }

    [MaxLength(20)]
    public string? ContactPhone { get; set; }

    [MaxLength(100)]
    public string? ContactEmail { get; set; }

    public bool IsOpen { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
