using PriceParcer.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceParcer.Core.Interfaces
{
    public interface IProductPricesService
    {
        Task<ProductPriceDTO> ParseProductPriceAsync(Guid productFromSitesId);
        Task<IEnumerable<ProductPriceDTO>> GetAllProductPricesAsync(Guid productFromSitesId);
        Task<ProductPriceDTO> GetProductPriceDetailsAsync(Guid priceId);
        Task<bool> AddProductPriceAsync(ProductPriceDTO productPriceDTO);
        Task<bool> UpdateProductPriceAsync(ProductPriceDTO productPriceDTO);
        Task<bool> DeleteProductPriceAsync(Guid id);


    }
}
