using System.ComponentModel.DataAnnotations;

namespace FestHubCentral.Web.Data.Models;

public class EventLocation
{
    public int Id { get; set; }

    [Required]
    public int EventYear { get; set; }
    public Event Event { get; set; } = null!;

    [Required]
    public int LocationId { get; set; }
    public Location Location { get; set; } = null!;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
