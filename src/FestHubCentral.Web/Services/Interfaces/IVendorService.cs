using FestHubCentral.Web.Data.Models;

namespace FestHubCentral.Web.Services.Interfaces;

public interface IVendorService
{
    Task<IEnumerable<Vendor>> GetAllVendorsAsync();
    Task<Vendor?> GetVendorByIdAsync(int id);
    Task<Vendor> CreateVendorAsync(Vendor vendor);
    Task<Vendor> UpdateVendorAsync(Vendor vendor);
    Task DeleteVendorAsync(int id);
}
