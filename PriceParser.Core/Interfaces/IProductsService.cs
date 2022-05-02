using PriceParser.Core.DTO;
using PriceParser.Data.Entities;

namespace PriceParser.Core.Interfaces
{
    public interface IProductsService
    {
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync(int pageNumber);
        Task<ProductDTO> GetProductDetailsAsync(Guid id);

        Task<bool> AddProductAsync(ProductDTO product);

        Task<bool> EditProductAsync(ProductDTO product);

        Task<bool> DeleteProductAsync(Guid id);

        Task<bool> UpdateAggregatedPricesDataAsync(Guid Id);
        Task<bool> UpdateAggregatedPricesDataAsync(Product product);
        Task<bool> UpdateAggregatedPricesDataAsync();

        Task<bool> UpdateAggregatedReviewRateDataAsync(Guid Id);
        Task<bool> UpdateAggregatedReviewRateDataAsync(Product product);
        Task<bool> UpdateAggregatedReviewRateDataAsync();

    }
}
