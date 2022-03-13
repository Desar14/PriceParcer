using PriceParser.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceParser.Core.Interfaces
{
    public interface IProductsService
    {
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
        Task<ProductDTO> GetProductDetailsAsync(Guid id);

        Task<bool> AddProduct(ProductDTO product);

        Task<bool> EditProduct(ProductDTO product);

        Task<bool> DeleteProduct(Guid id);

    }
}
