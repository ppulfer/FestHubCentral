using FestHubCentral.Web.Data;
using FestHubCentral.Web.Data.Models;
using FestHubCentral.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FestHubCentral.Web.Services;

public class CashRegisterService : ICashRegisterService
{
    private readonly ApplicationDbContext _context;

    public CashRegisterService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CashRegister>> GetAllCashRegistersAsync()
    {
        return await _context.CashRegisters
            .Include(cr => cr.Vendor)
            .OrderByDescending(cr => cr.OpenedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<CashRegister>> GetOpenCashRegistersAsync()
    {
        return await _context.CashRegisters
            .Include(cr => cr.Vendor)
            .Where(cr => cr.IsOpen)
            .ToListAsync();
    }

    public async Task<CashRegister?> GetCashRegisterByIdAsync(int id)
    {
        return await _context.CashRegisters
            .Include(cr => cr.Vendor)
            .FirstOrDefaultAsync(cr => cr.Id == id);
    }

    public async Task<CashRegister?> GetOpenCashRegisterByVendorAsync(int vendorId)
    {
        return await _context.CashRegisters
            .Where(cr => cr.VendorId == vendorId && cr.IsOpen)
            .FirstOrDefaultAsync();
    }

    public async Task<CashRegister> OpenCashRegisterAsync(CashRegister cashRegister)
    {
        cashRegister.OpenedAt = DateTime.UtcNow;
        cashRegister.IsOpen = true;
        _context.CashRegisters.Add(cashRegister);
        await _context.SaveChangesAsync();
        return cashRegister;
    }

    public async Task<CashRegister> CloseCashRegisterAsync(int id, decimal closingAmount, string closedBy)
    {
        var cashRegister = await _context.CashRegisters.FindAsync(id);
        if (cashRegister != null)
        {
            var expectedAmount = await CalculateExpectedAmountAsync(id);

            cashRegister.ClosingAmount = closingAmount;
            cashRegister.ExpectedAmount = expectedAmount;
            cashRegister.Discrepancy = closingAmount - expectedAmount;
            cashRegister.IsOpen = false;
            cashRegister.ClosedAt = DateTime.UtcNow;
            cashRegister.ClosedBy = closedBy;

            await _context.SaveChangesAsync();
        }
        return cashRegister!;
    }

    public async Task<decimal> CalculateExpectedAmountAsync(int cashRegisterId)
    {
        var cashRegister = await _context.CashRegisters.FindAsync(cashRegisterId);
        if (cashRegister == null) return 0;

        var orders = await _context.Orders
            .Where(o => o.VendorId == cashRegister.VendorId
                && o.OrderDate >= cashRegister.OpenedAt
                && (!cashRegister.ClosedAt.HasValue || o.OrderDate <= cashRegister.ClosedAt.Value))
            .ToListAsync();

        cashRegister.CashSales = orders.Where(o => o.PaymentMethod == "Cash").Sum(o => o.TotalAmount);
        cashRegister.CardSales = orders.Where(o => o.PaymentMethod == "Card").Sum(o => o.TotalAmount);
        cashRegister.TokenSales = orders.Where(o => o.PaymentMethod == "FestivalToken").Sum(o => o.TotalAmount);

        return cashRegister.OpeningAmount + cashRegister.CashSales;
    }
}
