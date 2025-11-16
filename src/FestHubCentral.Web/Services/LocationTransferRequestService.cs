using Microsoft.EntityFrameworkCore;
using FestHubCentral.Web.Data;
using FestHubCentral.Web.Data.Models;
using FestHubCentral.Web.Services.Interfaces;

namespace FestHubCentral.Web.Services;

public class LocationTransferRequestService : ILocationTransferRequestService
{
    private readonly ApplicationDbContext _context;
    private readonly IInventoryTransferService _inventoryTransferService;

    public LocationTransferRequestService(ApplicationDbContext context, IInventoryTransferService inventoryTransferService)
    {
        _context = context;
        _inventoryTransferService = inventoryTransferService;
    }

    public async Task<IEnumerable<TransferRequest>> GetAllRequestsAsync()
    {
        return await _context.TransferRequests
            .Include(tr => tr.Product)
            .Include(tr => tr.FromLocation)
            .Include(tr => tr.ToLocation)
            .Include(tr => tr.RequestedByUser)
            .Include(tr => tr.ReviewedByUser)
            .OrderByDescending(tr => tr.RequestedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<TransferRequest>> GetPendingRequestsAsync()
    {
        return await _context.TransferRequests
            .Where(tr => tr.Status == TransferRequestStatus.Pending)
            .Include(tr => tr.Product)
            .Include(tr => tr.FromLocation)
            .Include(tr => tr.ToLocation)
            .Include(tr => tr.RequestedByUser)
            .OrderByDescending(tr => tr.RequestedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<TransferRequest>> GetRequestsByLocationAsync(int locationId)
    {
        return await _context.TransferRequests
            .Where(tr => tr.FromLocationId == locationId)
            .Include(tr => tr.Product)
            .Include(tr => tr.FromLocation)
            .Include(tr => tr.ToLocation)
            .Include(tr => tr.RequestedByUser)
            .OrderByDescending(tr => tr.RequestedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<TransferRequest>> GetRequestsByLocationAndYearAsync(int locationId, int eventYear)
    {
        return await _context.TransferRequests
            .Where(tr => tr.FromLocationId == locationId && tr.EventYear == eventYear)
            .Include(tr => tr.Product)
            .Include(tr => tr.FromLocation)
            .Include(tr => tr.ToLocation)
            .Include(tr => tr.RequestedByUser)
            .OrderByDescending(tr => tr.RequestedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<TransferRequest>> GetRequestsByStatusAsync(TransferRequestStatus status)
    {
        return await _context.TransferRequests
            .Where(tr => tr.Status == status)
            .Include(tr => tr.Product)
            .Include(tr => tr.FromLocation)
            .Include(tr => tr.ToLocation)
            .Include(tr => tr.RequestedByUser)
            .OrderByDescending(tr => tr.RequestedAt)
            .ToListAsync();
    }

    public async Task<TransferRequest?> GetRequestByIdAsync(int id)
    {
        return await _context.TransferRequests
            .Include(tr => tr.Product)
            .Include(tr => tr.FromLocation)
            .Include(tr => tr.ToLocation)
            .Include(tr => tr.RequestedByUser)
            .Include(tr => tr.ReviewedByUser)
            .FirstOrDefaultAsync(tr => tr.Id == id);
    }

    public async Task<TransferRequest> CreateRequestAsync(TransferRequest request)
    {
        request.Status = TransferRequestStatus.Pending;
        request.RequestedAt = DateTime.UtcNow;

        _context.TransferRequests.Add(request);
        await _context.SaveChangesAsync();

        return request;
    }

    public async Task<TransferRequest> ApproveRequestAsync(int requestId, string approvedByUserId)
    {
        var request = await GetRequestByIdAsync(requestId);
        if (request == null)
            throw new InvalidOperationException($"Transfer request with ID {requestId} not found.");

        if (request.Status != TransferRequestStatus.Pending)
            throw new InvalidOperationException($"Cannot approve request with status {request.Status}.");

        var transfer = new InventoryTransfer
        {
            ProductId = request.ProductId,
            FromLocationId = request.FromLocationId,
            ToLocationId = request.ToLocationId,
            Amount = request.Amount,
            Comment = request.Comment,
            EventYear = request.EventYear,
            TransferDate = DateTime.UtcNow,
            CreatedByUserId = approvedByUserId,
            CreatedAt = DateTime.UtcNow
        };

        _context.InventoryTransfers.Add(transfer);
        await _context.SaveChangesAsync();

        request.Status = TransferRequestStatus.Approved;
        request.ApprovedByInventoryTransferId = transfer.Id;
        request.ReviewedAt = DateTime.UtcNow;
        request.ReviewedByUserId = approvedByUserId;

        _context.TransferRequests.Update(request);
        await _context.SaveChangesAsync();

        return request;
    }

    public async Task<TransferRequest> RejectRequestAsync(int requestId, string rejectedByUserId, string rejectionReason)
    {
        var request = await GetRequestByIdAsync(requestId);
        if (request == null)
            throw new InvalidOperationException($"Transfer request with ID {requestId} not found.");

        if (request.Status != TransferRequestStatus.Pending)
            throw new InvalidOperationException($"Cannot reject request with status {request.Status}.");

        request.Status = TransferRequestStatus.Rejected;
        request.RejectionReason = rejectionReason;
        request.ReviewedAt = DateTime.UtcNow;
        request.ReviewedByUserId = rejectedByUserId;

        _context.TransferRequests.Update(request);
        await _context.SaveChangesAsync();

        return request;
    }

    public async Task<bool> DeleteRequestAsync(int id)
    {
        var request = await _context.TransferRequests.FindAsync(id);
        if (request == null)
            return false;

        _context.TransferRequests.Remove(request);
        await _context.SaveChangesAsync();
        return true;
    }
}
