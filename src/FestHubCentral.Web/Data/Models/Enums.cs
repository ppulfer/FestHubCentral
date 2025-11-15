namespace FestHubCentral.Web.Data.Models;

public enum VendorCategory
{
    Food,
    Beverage,
    Mixed
}

public enum PaymentMethod
{
    Cash,
    Card,
    FestivalToken
}

public enum AlertType
{
    LowInventory,
    CriticalInventory,
    VendorStatus,
    CashDiscrepancy,
    System
}

public enum AlertSeverity
{
    Info,
    Warning,
    Critical
}

public enum UnitType
{
    Piece,
    Glass,
    Bottle,
    Plate,
    Portion,
    Liter,
    Kilogram
}
