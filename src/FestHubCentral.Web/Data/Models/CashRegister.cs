using System.ComponentModel.DataAnnotations;

namespace FestHubCentral.Web.Data.Models;

public class CashRegister
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string RegisterName { get; set; } = string.Empty;

    [Required]
    public int LocationId { get; set; }
    public Location Location { get; set; } = null!;

    [Required]
    [Range(0, 1000000)]
    public decimal OpeningAmount { get; set; }

    [Range(0, 1000000)]
    public decimal? ClosingAmount { get; set; }

    [Range(0, 1000000)]
    public decimal ExpectedAmount { get; set; }

    [Range(-100000, 100000)]
    public decimal Discrepancy { get; set; }

    [Range(0, 1000000)]
    public decimal CashSales { get; set; }

    [Range(0, 1000000)]
    public decimal CardSales { get; set; }

    [Range(0, 1000000)]
    public decimal TokenSales { get; set; }

    public bool IsOpen { get; set; } = true;

    public DateTime OpenedAt { get; set; }
    public DateTime? ClosedAt { get; set; }

    [MaxLength(100)]
    public string? OpenedBy { get; set; }

    [MaxLength(100)]
    public string? ClosedBy { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }
}
