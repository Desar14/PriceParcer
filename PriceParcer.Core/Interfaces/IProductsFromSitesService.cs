using PriceParcer.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceParcer.Core.Interfaces
{
    public interface IProductsFromSitesService
    {
        Task<IEnumerable<ProductFromSitesDTO>> GetAllAsync();

        Task<IEnumerable<ProductFromSitesDTO>> GetAllBySiteAsync(Guid siteId);

        Task<IEnumerable<ProductFromSitesDTO>> GetAllByProductAsync(Guid productId);

        Task<ProductFromSitesDTO> GetDetailsAsync(Guid id);

        Task<bool> AddAsync(ProductFromSitesDTO product);

        Task<bool> EditAsync(ProductFromSitesDTO product);

        Task<bool> DeleteAsync(Guid id);

    }
}
