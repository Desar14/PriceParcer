using PriceParser.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceParser.Core.Interfaces
{
    public interface IProductPricesService
    {
        
        Task<IEnumerable<ProductPriceDTO>> GetAllProductPricesAsync(Guid productFromSitesId);
        Task<IEnumerable<ProductPriceDTO>> GetAllProductFromSitePricesAsync(Guid productFromSitesId, DateTime? startDate, DateTime? endDate, bool perEveryDay = false);
        Task<IEnumerable<ProductPriceDTO>> GetAllProductPricesAsync(Guid productId, DateTime? startDate, DateTime? endDate, bool perEveryDay = false);
        Task<ProductPriceDTO> GetLastProductPriceAsync(Guid productFromSitesId);
        Task<ProductPriceDTO> GetProductPriceDetailsAsync(Guid priceId);

        Task<bool> AddProductPriceAsync(ProductPriceDTO productPriceDTO);
        Task<bool> AddProductPricesRangeAsync(IEnumerable<ProductPriceDTO> productPriceDTORange);

        Task<bool> UpdateProductPriceAsync(ProductPriceDTO productPriceDTO);
        Task<bool> DeleteProductPriceAsync(Guid id);        
    }
}
