using AutoMapper;
using Microsoft.Extensions.Logging;
using PriceParser.Core.DTO;
using PriceParser.Core.Interfaces;
using PriceParser.Data.Entities;
using System.Linq.Expressions;

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

            var lastPricesPerProdFromSiteId = (await _unitOfWork.ProductPricesHistory.GetQueryable())
                .Where(x => x.FullPrice != 0)
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
                        });

            var entityRangeFiltered = entityRange.Where(x => !lastPricesPerProdFromSiteId.Any(y => x.ProductFromSiteId == y.ProdFromSiteId && x.FullPrice == y.CurrentPrice));

            await _unitOfWork.ProductPricesHistory.AddRange(entityRangeFiltered);

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

        public async Task<IEnumerable<ProductPriceDTO>> GetAllProductFromSitePricesAsync(Guid productFromSitesId, DateTime? startDate, DateTime? endDate, bool perEveryDay = false)
        {
            return await GetPricesAsync(default, productFromSitesId, startDate, endDate, perEveryDay);
        }

        public async Task<ProductPriceDTO> GetLastProductPriceAsync(Guid productFromSitesId)
        {
            var result = (await _unitOfWork.ProductPricesHistory.GetQueryable())
                .Where(price => price.ProductFromSiteId == productFromSitesId)
                .OrderByDescending(x=> x.ParseDate)
                .Take(1)
                .FirstOrDefault();

            return _mapper.Map<ProductPriceDTO>(result);
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

        public async Task<IEnumerable<ProductPriceDTO>> GetAllProductPricesAsync(Guid productId, DateTime? startDate, DateTime? endDate, bool perEveryDay = false)
        {
            return await GetPricesAsync(productId, default, startDate, endDate, perEveryDay);
        }

        async Task<IEnumerable<ProductPriceDTO>> GetPricesAsync(Guid productId, Guid productFromSiteId, DateTime? startDate, DateTime? endDate, bool perEveryDay = false)
        {
            if (startDate == null)
            {
                startDate = DateTime.MinValue;
                perEveryDay = false;
            }


            if (endDate == null)
            {
                endDate = DateTime.MinValue;
                perEveryDay = false;
            }

            //start of day and end of day
            startDate = startDate?.Date;
            endDate = endDate?.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            Expression<Func<ProductPrice, bool>>? filter;

            if (productId != default && productFromSiteId == default)
            {
                filter = price =>
                    price.ProductFromSite.ProductId == productId
                    && price.ParseDate >= startDate
                    && price.ParseDate <= endDate
                    && !price.ParseError;
            }
            else if (productFromSiteId != default && productId == default)
            {
                filter = price =>
                    price.ProductFromSiteId == productFromSiteId
                    && price.ParseDate >= startDate
                    && price.ParseDate <= endDate
                    && !price.ParseError;
            }
            else
                throw new ArgumentException("Incorrect id arguments!");

            

            var prices = (await _unitOfWork.ProductPricesHistory
                .Get(filter: filter,
                    includes: price => price.ProductFromSite))
                .Select(price => _mapper.Map<ProductPriceDTO>(price));

            if (perEveryDay && startDate != null && endDate != null)
            {
                prices = MakePricesPerEveryDay(prices, startDate.Value, endDate.Value);
            }

            return prices;
        }

        IEnumerable<ProductPriceDTO> MakePricesPerEveryDay(IEnumerable<ProductPriceDTO> productPrices, DateTime startDate, DateTime endDate)
        {

            startDate = startDate.Date;
            endDate = endDate.Date;

            List<DateTime> dates = new List<DateTime>();
            for (DateTime i = startDate; i <= endDate; i = i.AddDays(1))
            {
                dates.Add(i);
            }

            var groupedPricesInDay = productPrices.GroupBy(
                g => _mapper.Map<ProductPriceDTO>(g, opt => opt.AfterMap((src, dest) => dest.ParseDate = dest.ParseDate.Date)), 
                c => c.FullPrice, 
                (g, c) => _mapper.Map<ProductPriceDTO>(g, opt => opt.AfterMap((src, dest) => dest.FullPrice = c.Average()))
                );

            var datesForJoining = groupedPricesInDay
                .Join(dates, t1 => 1, t2 => 1, (t1, t2) => new
                {
                    t1.ProductFromSiteId,
                    t1.ParseDate,
                    ResultDate = t2
                })
                .Where(x=> x.ResultDate >= x.ParseDate)
                .GroupBy(g => new { g.ProductFromSiteId, g.ResultDate }, c=> c.ParseDate,
                (g,c) => new
                {
                    g.ProductFromSiteId,
                    g.ResultDate,
                    ParseDate = c.Max()
                })
                .OrderBy(x => x.ResultDate);

            var result = datesForJoining.Join(groupedPricesInDay,
                t1 => new { t1.ProductFromSiteId, t1.ParseDate },
                t2 => new { t2.ProductFromSiteId, t2.ParseDate },
                (t1, t2) => _mapper.Map<ProductPriceDTO>(t2, opt => opt.AfterMap((src, dest) => dest.ParseDate = t1.ResultDate)));                

            return result;
        }
    }
}
