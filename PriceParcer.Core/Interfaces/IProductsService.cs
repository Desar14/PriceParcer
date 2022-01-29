using PriceParcer.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceParcer.Core.Interfaces
{
    public interface IProductsService
    {
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
    }
}
