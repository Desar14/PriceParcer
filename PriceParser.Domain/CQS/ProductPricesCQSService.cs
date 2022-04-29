using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PriceParser.Core.DTO;
using PriceParser.Core.Interfaces;
using PriceParser.CQS.Models.Queries;
using PriceParser.CQS.Models.Commands;

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

        public async Task<IEnumerable<ProductFromSitesDTO>> GetAllProductFromSitePricesAsync(Guid productFromSitesId, DateTime? startDate, DateTime? endDate, bool perEveryDay = false)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ProductPriceDTO>> GetAllProductPricesAsync(Guid productFromSitesId)
        {
            return await _mediator.Send(new GetAllProductPricesQuery(productFromSitesId), new CancellationToken());
        }

        public async Task<IEnumerable<ProductFromSitesDTO>> GetAllProductPricesAsync(Guid productId, DateTime? startDate, DateTime? endDate, bool perEveryDay = false)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ProductFromSitesDTO>> GetAllProductPricesPerSiteAsync(Guid productId, DateTime? startDate, DateTime? endDate, bool perEveryDay = false)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductPriceDTO> GetLastProductPriceAsync(Guid productFromSitesId)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductPriceDTO> GetProductPriceDetailsAsync(Guid priceId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateProductPriceAsync(ProductPriceDTO productPriceDTO)
        {
            return await _mediator.Send(new UpdateProductPriceCommand(productPriceDTO), new CancellationToken());
        }
    }
}
