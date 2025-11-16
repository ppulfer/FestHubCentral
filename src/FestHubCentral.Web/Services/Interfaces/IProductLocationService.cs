using FestHubCentral.Web.Data.Models;

namespace FestHubCentral.Web.Services.Interfaces;

public interface IProductLocationService
{
    Task<IEnumerable<ProductLocation>> GetAllProductLocationsAsync();
    Task<IEnumerable<ProductLocation>> GetProductLocationsByEventYearAsync(int eventYear);
    Task<IEnumerable<ProductLocation>> GetProductLocationsByLocationAsync(int locationId, int eventYear);
    Task<ProductLocation?> GetProductLocationByIdAsync(int id);
    Task<ProductLocation?> GetProductLocationAsync(int productId, int locationId, int eventYear);
    Task<ProductLocation> CreateProductLocationAsync(ProductLocation productLocation);
    Task<ProductLocation> UpdateProductLocationAsync(ProductLocation productLocation);
    Task<bool> DeleteProductLocationAsync(int id);
    Task<bool> ProductLocationExistsAsync(int productId, int locationId, int eventYear);
}
