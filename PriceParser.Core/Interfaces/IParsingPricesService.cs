using PriceParser.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
