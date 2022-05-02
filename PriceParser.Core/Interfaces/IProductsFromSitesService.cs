using PriceParser.Core.DTO;

namespace PriceParser.Core.Interfaces
{
    public interface IProductsFromSitesService
    {
        Task<IEnumerable<ProductFromSitesDTO>> GetAllAsync();
        Task<IEnumerable<ProductFromSitesDTO>> GetAllBySiteAsync(Guid siteId);
        Task<IEnumerable<ProductFromSitesDTO>> GetBySiteForParsingAsync(Guid siteId);
        Task<IEnumerable<ProductFromSitesDTO>> GetAllByProductAsync(Guid productId);
        Task<ProductFromSitesDTO> GetDetailsAsync(Guid id);

        Task<bool> AddAsync(ProductFromSitesDTO product);

        Task<bool> EditAsync(ProductFromSitesDTO product);

        Task<bool> DeleteAsync(Guid id);
    }
}
