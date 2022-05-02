using PriceParser.Core.DTO;

namespace PriceParser.Core.Interfaces
{
    public interface IProductPricesService
    {

        Task<IEnumerable<ProductPriceDTO>> GetAllProductPricesAsync(Guid productFromSitesId);
        Task<IEnumerable<ProductFromSitesDTO>> GetAllProductFromSitePricesAsync(Guid productFromSitesId, DateTime? startDate, DateTime? endDate, bool perEveryDay = false);
        Task<IEnumerable<ProductFromSitesDTO>> GetAllProductPricesAsync(Guid productId, DateTime? startDate, DateTime? endDate, bool perEveryDay = false);
        Task<IEnumerable<ProductFromSitesDTO>> GetAllProductPricesPerSiteAsync(Guid productId, DateTime? startDate, DateTime? endDate, bool perEveryDay = false);
        Task<ProductPriceDTO> GetLastProductPriceAsync(Guid productFromSitesId);
        Task<ProductPriceDTO> GetProductPriceDetailsAsync(Guid priceId);

        Task<bool> AddProductPriceAsync(ProductPriceDTO productPriceDTO);
        Task<bool> AddProductPricesRangeAsync(IEnumerable<ProductPriceDTO> productPriceDTORange);

        Task<bool> UpdateProductPriceAsync(ProductPriceDTO productPriceDTO);
        Task<bool> DeleteProductPriceAsync(Guid id);
    }
}
