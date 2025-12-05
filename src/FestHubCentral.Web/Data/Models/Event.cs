using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FestHubCentral.Web.Data.Models;

public class Event
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Year { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsPassed { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
    public ICollection<ProductEventPrice> ProductEventPrices { get; set; } = new List<ProductEventPrice>();
    public ICollection<EventLocation> EventLocations { get; set; } = new List<EventLocation>();
}
