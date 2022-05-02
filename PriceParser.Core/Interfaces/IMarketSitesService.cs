using PriceParser.Core.DTO;

namespace PriceParser.Core.Interfaces
{
    public interface IMarketSitesService
    {
        Task<IEnumerable<MarketSiteDTO>> GetAllSitesAsync();
        Task<IEnumerable<MarketSiteDTO>> GetOnlyAvailableSitesAsync();
        Task<MarketSiteDTO> GetSiteDetailsAsync(Guid id);

        Task<bool> AddSite(MarketSiteDTO product);

        Task<bool> EditSite(MarketSiteDTO product);

        Task<bool> DeleteSite(Guid id);
    }
}
