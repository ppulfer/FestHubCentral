using FestHubCentral.Web.Data.Models;

namespace FestHubCentral.Web.Services.Interfaces;

public interface ILocationTransferRequestService
{
    Task<IEnumerable<TransferRequest>> GetAllRequestsAsync();
    Task<IEnumerable<TransferRequest>> GetPendingRequestsAsync();
    Task<IEnumerable<TransferRequest>> GetRequestsByLocationAsync(int locationId);
    Task<IEnumerable<TransferRequest>> GetRequestsByLocationAndYearAsync(int locationId, int eventYear);
    Task<IEnumerable<TransferRequest>> GetRequestsByStatusAsync(TransferRequestStatus status);
    Task<TransferRequest?> GetRequestByIdAsync(int id);
    Task<TransferRequest> CreateRequestAsync(TransferRequest request);
    Task<TransferRequest> ApproveRequestAsync(int requestId, string approvedByUserId);
    Task<TransferRequest> RejectRequestAsync(int requestId, string rejectedByUserId, string rejectionReason);
    Task<bool> DeleteRequestAsync(int id);
}
