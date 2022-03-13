using PriceParser.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceParser.Core.Interfaces
{
    public interface IMarketSitesService
    {
        Task<IEnumerable<MarketSiteDTO>> GetAllSitesAsync();
        Task<MarketSiteDTO> GetSiteDetailsAsync(Guid id);

        Task<bool> AddSite(MarketSiteDTO product);

        Task<bool> EditSite(MarketSiteDTO product);

        Task<bool> DeleteSite(Guid id);
    }
}
