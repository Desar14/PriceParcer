using MediatR;
using Microsoft.Extensions.Logging;
using PriceParser.Core.DTO;
using PriceParser.Core.Interfaces;
using PriceParser.CQS.Models.Commands;
using PriceParser.CQS.Models.Queries;

namespace PriceParser.Domain.CQS
{
    public class ProductPricesCQSService : IProductPricesService
    {
        private readonly ILogger<ProductPricesService> _logger;
        private readonly IMediator _mediator;

        public ProductPricesCQSService(ILogger<ProductPricesService> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<bool> AddProductPriceAsync(ProductPriceDTO productPriceDTO)
        {
            return await _mediator.Send(new AddProductPriceCommand(productPriceDTO), new CancellationToken());
        }

        public async Task<bool> AddProductPricesRangeAsync(IEnumerable<ProductPriceDTO> productPriceDTORange)
        {
            return await _mediator.Send(new AddProductPriceRangeCommand(productPriceDTORange), new CancellationToken());
        }

        public async Task<bool> DeleteProductPriceAsync(Guid id)
        {
            return await _mediator.Send(new DeleteProductPriceCommand(id), new CancellationToken());
        }

        public async Task<IEnumerable<ProductPriceDTO>> GetAllProductPricesAsync(Guid productFromSitesId)
        {
            return await _mediator.Send(new GetAllProductPricesQuery(productFromSitesId), new CancellationToken());
        }

        public async Task<IEnumerable<ProductFromSitesDTO>> GetAllProductFromSitePricesAsync(Guid productFromSitesId, DateTime? startDate, DateTime? endDate, bool perEveryDay = false)
        {
            var prices = await _mediator.Send(new GetProductFromSitePricesQuery()
            {
                StartDate = startDate,
                EndDate = endDate,
                PerEveryDay = perEveryDay,
                ProductFromSiteId = productFromSitesId,
                ProductId = default
            }, new CancellationToken());

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

        public async Task<IEnumerable<ProductFromSitesDTO>> GetAllProductPricesAsync(Guid productId, DateTime? startDate, DateTime? endDate, bool perEveryDay = false)
        {
            var averageData = await _mediator.Send(new GetProductPricesAverageQuery()
            {
                StartDate = startDate,
                EndDate = endDate,
                PerEveryDay = perEveryDay,
                ProductId = productId
            }, new CancellationToken());

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
        public async Task<IEnumerable<ProductFromSitesDTO>> GetAllProductPricesPerSiteAsync(Guid productId, DateTime? startDate, DateTime? endDate, bool perEveryDay = false)
        {
            var agg = (await _mediator.Send(new GetAllProductPricesPerSiteQuery()
            {
                StartDate = startDate,
                EndDate = endDate,
                PerEveryDay = perEveryDay,
                ProductId = productId
            }, new CancellationToken())).ToList();


            var averageData = await GetAllProductPricesAsync(productId, startDate, endDate, perEveryDay);

            if (averageData.Any())
            {
                agg.AddRange(averageData);
            }

            return agg;
        }

        public async Task<ProductPriceDTO> GetLastProductPriceAsync(Guid productFromSitesId)
        {
            return await _mediator.Send(new GetLastProductPriceQuery(productFromSitesId), new CancellationToken());
        }

        public async Task<ProductPriceDTO> GetProductPriceDetailsAsync(Guid priceId)
        {
            return await _mediator.Send(new GetProductPriceDetailsQuery(priceId), new CancellationToken());
        }

        public async Task<bool> UpdateProductPriceAsync(ProductPriceDTO productPriceDTO)
        {
            return await _mediator.Send(new UpdateProductPriceCommand(productPriceDTO), new CancellationToken());
        }
    }
}
