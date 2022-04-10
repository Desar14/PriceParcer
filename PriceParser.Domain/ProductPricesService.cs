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
        private readonly IProductsFromSitesService _productFromSitesService;
        private readonly ILogger<ProductPricesService> _logger;

        public ProductPricesService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ProductPricesService> logger, IProductsFromSitesService productFromSitesService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _productFromSitesService = productFromSitesService;
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
                        maxDates.ProdFromSiteId,
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

        public async Task<IEnumerable<ProductFromSitesDTO>> GetAllProductFromSitePricesAsync(Guid productFromSitesId, DateTime? startDate, DateTime? endDate, bool perEveryDay = false)
        {

            var prices = await GetPricesAsync(default, productFromSitesId, startDate, endDate, perEveryDay);

            var result = new List<ProductFromSitesDTO>()
            {
                new ProductFromSitesDTO()
                {
                    Id = productFromSitesId,
                    Site = new() { Name = "Average" },
                    Prices = prices.ToList()
                }
            };

            return result;
        }

        public async Task<ProductPriceDTO> GetLastProductPriceAsync(Guid productFromSitesId)
        {
            var result = (await _unitOfWork.ProductPricesHistory.GetQueryable())
                .Where(price => price.ProductFromSiteId == productFromSitesId)
                .OrderByDescending(x => x.ParseDate)
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

        public async Task<IEnumerable<ProductFromSitesDTO>> GetAllProductPricesAsync(Guid productId, DateTime? startDate, DateTime? endDate, bool perEveryDay = false)
        {
            var prices = await GetPricesAsync(productId, default, startDate, endDate, perEveryDay);

            var averageData = prices
                .GroupBy(
                g => new
                {
                    ParseDate = g.ParseDate.Date,
                    g.DiscountPrice,
                    g.DiscountPercent,
                    g.CurrencyCode,
                    g.CurrencyId,
                    g.IsOutOfStock,
                    g.ParseError
                },
                c => c.FullPrice,
                (g, c) => new ProductPriceDTO()
                {
                    Id = Guid.Empty,
                    ProductFromSiteId = Guid.Empty,
                    ParseDate = g.ParseDate,
                    FullPrice = c.Average(),
                    DiscountPrice = g.DiscountPrice,
                    DiscountPercent = g.DiscountPercent,
                    CurrencyCode = g.CurrencyCode,
                    CurrencyId = g.CurrencyId,
                    IsOutOfStock = g.IsOutOfStock,
                    ParseError = g.ParseError
                }
                );

            var result = new List<ProductFromSitesDTO>()
            {
                new ProductFromSitesDTO()
                {
                    Site = new() { Name = "Average" },
                    Prices = averageData.ToList()
                }
            };

            return result;
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
                return MakePricesPerEveryDay(prices, startDate.Value, endDate.Value);
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

            //just an awful code, but looks like group by needs anonymous types, and automapper can't work with them
            var groupedPricesInDay = productPrices
                .GroupBy(
                g => new
                {
                    g.ProductFromSiteId,
                    ParseDate = g.ParseDate.Date,
                    g.DiscountPrice,
                    g.DiscountPercent,
                    g.CurrencyCode,
                    g.CurrencyId,
                    g.IsOutOfStock,
                    g.ParseError
                },
                c => c.FullPrice,
                (g, c) => new ProductPriceDTO()
                {
                    Id = Guid.Empty,
                    ProductFromSiteId = g.ProductFromSiteId,
                    ParseDate = g.ParseDate,
                    FullPrice = c.Average(),
                    DiscountPrice = g.DiscountPrice,
                    DiscountPercent = g.DiscountPercent,
                    CurrencyCode = g.CurrencyCode,
                    CurrencyId = g.CurrencyId,
                    IsOutOfStock = g.IsOutOfStock,
                    ParseError = g.ParseError
                }
                );

            //var groupedPricesInDay1 = productPrices
            //    .GroupBy(
            //    g => _mapper.Map<ProductPriceDTO>(g, opt => 
            //        opt.AfterMap((src, dest) => {
            //            dest.ParseDate = dest.ParseDate.Date;
            //            dest.Id = Guid.Empty;
            //        })              
            //        ),
            //    c => c.FullPrice,
            //    (g, c) => _mapper.Map<ProductPriceDTO>(g, opt => opt.AfterMap((src, dest) => dest.FullPrice = c.Average())));

            var datesForJoining = groupedPricesInDay
                .Join(dates, t1 => 1, t2 => 1, (t1, t2) => new
                {
                    t1.ProductFromSiteId,
                    t1.ParseDate,
                    ResultDate = t2
                })
                .Where(x => x.ResultDate >= x.ParseDate)
                .GroupBy(g => new { g.ProductFromSiteId, g.ResultDate }, c => c.ParseDate,
                (g, c) => new
                {
                    g.ProductFromSiteId,
                    g.ResultDate,
                    ParseDate = c.Max()
                })
                .OrderBy(x => x.ResultDate);

            var result = datesForJoining.Join(groupedPricesInDay,
                t1 => new { t1.ProductFromSiteId, t1.ParseDate },
                t2 => new { t2.ProductFromSiteId, t2.ParseDate },
                (t1, t2) => new ProductPriceDTO()
                {
                    Id = Guid.Empty,
                    ProductFromSiteId = t2.ProductFromSiteId,
                    ParseDate = t1.ResultDate,
                    FullPrice = t2.FullPrice,
                    DiscountPrice = t2.DiscountPrice,
                    DiscountPercent = t2.DiscountPercent,
                    CurrencyCode = t2.CurrencyCode,
                    CurrencyId = t2.CurrencyId,
                    IsOutOfStock = t2.IsOutOfStock,
                    ParseError = t2.ParseError
                });            

            return result;
        }

        public async Task<IEnumerable<ProductFromSitesDTO>> GetAllProductPricesPerSiteAsync(Guid productId, DateTime? startDate, DateTime? endDate, bool perEveryDay = false)
        {
            var prices = await GetPricesAsync(productId, default, startDate, endDate, perEveryDay);

            var agg = prices.GroupBy(x => x.ProductFromSiteId).Select(x => new ProductFromSitesDTO
            {
                Id = x.Key,
                Prices = x.ToList()
            }).Join(
                await (_unitOfWork.ProductsFromSites.Get(x => x.ProductId == productId, null, x => x.Site)),
                x => x.Id,
                y => y.Id,
                (x, y) => _mapper.Map<ProductFromSitesDTO>(y, opt => opt.AfterMap((src, dest) => dest.Prices = x.Prices))
                ).ToList();

            var averageData = await GetAllProductPricesAsync(productId, startDate, endDate, perEveryDay);

            if (averageData.Any())
            {
                agg.AddRange(averageData);
            }            

            return agg;
        }
    }
}
