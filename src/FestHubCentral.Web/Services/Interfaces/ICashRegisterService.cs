using FestHubCentral.Web.Data.Models;

namespace FestHubCentral.Web.Services.Interfaces;

public interface ICashRegisterService
{
    Task<IEnumerable<CashRegister>> GetAllCashRegistersAsync();
    Task<IEnumerable<CashRegister>> GetOpenCashRegistersAsync();
    Task<CashRegister?> GetCashRegisterByIdAsync(int id);
    Task<CashRegister?> GetOpenCashRegisterByLocationAsync(int locationId);
    Task<CashRegister> OpenCashRegisterAsync(CashRegister cashRegister);
    Task<CashRegister> CloseCashRegisterAsync(int id, decimal closingAmount, string closedBy);
    Task<decimal> CalculateExpectedAmountAsync(int cashRegisterId);
}
