using AutoMapper;
using Microsoft.Extensions.Logging;
using PriceParser.Core.DTO;
using PriceParser.Core.Interfaces;
using PriceParser.Data.Entities;

namespace PriceParser.Domain
{
    public class ProductPricesService : IProductPricesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IProductsService _productsService;
        private readonly ILogger<ProductPricesService> _logger;

        public ProductPricesService(IUnitOfWork unitOfWork, IMapper mapper, IProductsService productsService, ILogger<ProductPricesService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productsService = productsService;
            _logger = logger;
        }

        public async Task<bool> AddProductPriceAsync(ProductPriceDTO productPriceDTO)
        {
            var entity = _mapper.Map<ProductPrice>(productPriceDTO);

            await _unitOfWork.ProductPricesHistory.Add(entity);

            var result = await _unitOfWork.Commit();

            return result > 0;
        }

        public async Task<bool> AddProductPricesRangeAsync(IEnumerable<ProductPriceDTO> productPriceDTORange)
        {
            var entityRange = productPriceDTORange.Select(x => _mapper.Map<ProductPrice>(x));

            var tempvar = (await _unitOfWork.ProductPricesHistory.GetQueryable())
                .Where(x => x.FullPrice != 0)
                .GroupBy(x => x.ProductFromSiteId, x => x.ParseDate, (prodId, date) => new
                {
                    ProdFromSiteId = prodId,
                    MaxDate = date.Max()
                })
                .Join(await _unitOfWork.ProductPricesHistory.GetQueryable(), maxDates => new { q1 = maxDates.ProdFromSiteId, q2 = maxDates.MaxDate },
                        rawTable => new { q1 = rawTable.ProductFromSiteId, q2 = rawTable.ParseDate }, (maxDates, rawTable) => new
                        {
                            ProdFromSiteId = maxDates.ProdFromSiteId,
                            CurrentPrice = rawTable.FullPrice
                        });
            //todo exclude exciting records
            
            await _unitOfWork.ProductPricesHistory.AddRange(entityRange);

            var result = await _unitOfWork.Commit();

            return result > 0;
        }

        public async Task<bool> DeleteProductPriceAsync(Guid id)
        {
            await _unitOfWork.ProductPricesHistory.Delete(id);

            var result = await _unitOfWork.Commit();

            return result > 0;
        }

        public async Task<IEnumerable<ProductPriceDTO>> GetAllProductPricesAsync(Guid productFromSitesId)
        {
            return (await _unitOfWork.ProductPricesHistory.Get(filter: price => price.ProductFromSiteId == productFromSitesId, includes: price => price.ProductFromSite))
                .Select(price => _mapper.Map<ProductPriceDTO>(price));
        }

        public async Task<IEnumerable<ProductPriceDTO>> GetAllProductPricesAsync(Guid productFromSitesId, DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null)
                startDate = DateTime.MinValue;

            if (endDate == null)
                endDate = DateTime.MaxValue;


            return (await _unitOfWork.ProductPricesHistory
                .Get(filter: price => price.ProductFromSiteId == productFromSitesId 
                    && price.ParseDate >= startDate 
                    && price.ParseDate <= endDate, 
                    includes: price => price.ProductFromSite))
                .Select(price => _mapper.Map<ProductPriceDTO>(price));
        }

        public async Task<ProductPriceDTO> GetProductPriceDetailsAsync(Guid priceId)
        {
            var result = (await _unitOfWork.ProductPricesHistory.GetByID(priceId, price => price.ProductFromSite));

            return _mapper.Map<ProductPriceDTO>(result);
        }


        public async Task<bool> UpdateProductPriceAsync(ProductPriceDTO productPriceDTO)
        {
            var entity = _mapper.Map<ProductPrice>(productPriceDTO);

            await _unitOfWork.ProductPricesHistory.Update(entity);

            var result = await _unitOfWork.Commit();

            return result > 0;
        }
    }
}
