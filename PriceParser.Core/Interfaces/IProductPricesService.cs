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
        Task<IEnumerable<ProductPriceDTO>> GetAllProductPricesAsync(Guid productFromSitesId, DateTime? startDate, DateTime? endDate);
        Task<ProductPriceDTO> GetLastProductPriceAsync(Guid productFromSitesId);
        Task<ProductPriceDTO> GetProductPriceDetailsAsync(Guid priceId);
        Task<bool> AddProductPriceAsync(ProductPriceDTO productPriceDTO);
        Task<bool> UpdateProductPriceAsync(ProductPriceDTO productPriceDTO);
        Task<bool> DeleteProductPriceAsync(Guid id);

        Task<bool> AddProductPricesRangeAsync(IEnumerable<ProductPriceDTO> productPriceDTORange);

    }
}
