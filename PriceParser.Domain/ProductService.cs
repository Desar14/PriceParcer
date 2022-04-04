using AutoMapper;
using Microsoft.Extensions.Logging;
using PriceParser.Core.DTO;
using PriceParser.Core.Interfaces;
using PriceParser.Data.Entities;

namespace PriceParser.Domain
{
    public class ProductService : IProductsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ProductService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            return (await _unitOfWork.Products.Get())
                .Select(product => _mapper.Map<ProductDTO>(product));
        }

        public async Task<ProductDTO> GetProductDetailsAsync(Guid id)
        {

            var result = (await _unitOfWork.Products.GetByID(id));

            if (result != null)
            {
                result.FromSites = new(await _unitOfWork.ProductsFromSites.Get(prod => prod.ProductId == id, null, prod => prod.Site));
                result.Reviews = new(await _unitOfWork.UserReviews.Get(prod => prod.ProductId == id, null, prod => prod.User));
            }

            return _mapper.Map<ProductDTO>(result);
        }

        public async Task<bool> UpdateAggregatedPricesData(Guid Id)
        {
            var aggOverallData = (await _unitOfWork.ProductPricesHistory.GetQueryable())
                .Where(x => x.ProductFromSite.ProductId == Id && x.FullPrice != 0)
                .GroupBy(x => 1)
                .Select(x => new
                {
                    AveragePrice = x.Average(x => x.FullPrice),
                    BestPrice = x.Min(x => x.FullPrice)
                }).FirstOrDefault();

            var aggNowData = (await _unitOfWork.ProductPricesHistory.GetQueryable())
                .Where(x => x.ProductFromSite.ProductId == Id && x.FullPrice != 0)
                .GroupBy(x => x.ProductFromSiteId, x => x.ParseDate, (prodId, date) => new
                {
                    ProdFromSiteId = prodId,
                    MaxDate = date.Max()
                })
                .Join(await _unitOfWork.ProductPricesHistory.GetQueryable(), 
                        maxDates => new { q1 = maxDates.ProdFromSiteId, q2 = maxDates.MaxDate },
                        rawTable => new { q1 = rawTable.ProductFromSiteId, q2 = rawTable.ParseDate }, 
                        (maxDates, rawTable) => new 
                        {
                            ProdFromSiteId = maxDates.ProdFromSiteId,
                            CurrentPrice = rawTable.FullPrice
                        })
                .GroupBy(x => 1)
                .Select(x => new
                {
                    AveragePrice = x.Average(x => x.CurrentPrice),
                    BestPrice = x.Min(x => x.CurrentPrice)
                }).FirstOrDefault();

            var productEntity = (await _unitOfWork.Products.FindBy(x=> x.Id.Equals(Id)));

            if (productEntity != null)
            {
                productEntity.AveragePriceOverall = aggOverallData.AveragePrice;
                productEntity.BestPriceOverall = aggOverallData.BestPrice;
                productEntity.AveragePriceNow = aggNowData.AveragePrice;
                productEntity.BestPriceNow = aggNowData.BestPrice;                

                productEntity.LastAggregate = DateTime.Now;

                await _unitOfWork.Products.Update(productEntity);
            }

            var result = await _unitOfWork.Commit();

            return result > 0;
        }

        public Task<bool> UpdateAggregatedPricesData(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAggregatedPricesData()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddProduct(ProductDTO product)
        {
            var entity = _mapper.Map<Product>(product);

            await _unitOfWork.Products.Add(entity);

            var result = await _unitOfWork.Commit();

            return result > 0;
        }

        public async Task<bool> DeleteProduct(Guid id)
        {
            await _unitOfWork.Products.Delete(id);

            var result = await _unitOfWork.Commit();

            return result > 0;
        }

        public async Task<bool> EditProduct(ProductDTO product)
        {
            var entity = _mapper.Map<Product>(product);

            await _unitOfWork.Products.Update(entity);

            var result = await _unitOfWork.Commit();

            return result > 0;
        }

        public async Task<bool> UpdateAggregatedReviewRateData(Guid Id)
        {
            var aggReviewRateAverage = (await _unitOfWork.UserReviews.GetQueryable())
                .Where(x => x.ProductId == Id)
                .Select(x => x.ReviewScore).DefaultIfEmpty().Average();

            var productEntity = (await _unitOfWork.Products.FindBy(x => x.Id.Equals(Id)));

            if (productEntity != null)
            {
                productEntity.AverageScore = (float)aggReviewRateAverage;

                await _unitOfWork.Products.Update(productEntity);
            }

            var result = await _unitOfWork.Commit();

            return result > 0;
        }

        public Task<bool> UpdateAggregatedReviewRateData(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAggregatedReviewRateData()
        {
            throw new NotImplementedException();
        }
    }
}