using PriceParser.Core.DTO;

namespace PriceParser.Core.Interfaces
{
    public interface IParsingPricesService
    {
        Task<ProductPriceDTO> ParseProductPriceAsync(Guid productFromSitesId);
        Task<ProductPriceDTO> ParseProductPriceAsync(ProductFromSitesDTO dto);

        Task<bool> ParseSaveProductPriceAsync(Guid productFromSitesId);
        Task<bool> ParseSaveAllAvailablePricesAsync();

    }
}
