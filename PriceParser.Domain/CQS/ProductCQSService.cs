using MediatR;
using Microsoft.Extensions.Logging;
using PriceParser.Core.DTO;
using PriceParser.Core.Interfaces;
using PriceParser.CQS.Models.Commands;
using PriceParser.CQS.Models.Queries;
using PriceParser.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceParser.Domain.CQS
{
    public class ProductCQSService : IProductsService
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProductService> _logger;

        public ProductCQSService(IMediator mediator, ILogger<ProductService> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<bool> AddProductAsync(ProductDTO product)
        {
            return await _mediator.Send(new AddProductCommand(product), new CancellationToken());
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            return await _mediator.Send(new DeleteProductCommand(id), new CancellationToken());
        }

        public async Task<bool> EditProductAsync(ProductDTO product)
        {
            return await _mediator.Send(new EditProductCommand(product), new CancellationToken());
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            return await _mediator.Send(new GetAllProductsQuery(), new CancellationToken());
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync(int pageNumber)
        {
            int pageSize = 10;
            return await _mediator.Send(new GetAllProductsQuery() { PageNumber = pageNumber, PageSize = pageSize}, new CancellationToken());
        }

        public async Task<ProductDTO> GetProductDetailsAsync(Guid id)
        {
            return await _mediator.Send(new GetProductDetailsQuery(id), new CancellationToken());
        }

        public async Task<bool> UpdateAggregatedPricesDataAsync(Guid Id)
        {
            return await _mediator.Send(new UpdateAggregatedPricesDataCommand(Id), new CancellationToken());
        }

        public  Task<bool> UpdateAggregatedPricesDataAsync(Product product)//not necessary
        {
            throw new NotImplementedException();
        }

        public  Task<bool> UpdateAggregatedPricesDataAsync()//not necessary
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAggregatedReviewRateDataAsync(Guid Id)
        {
            return await _mediator.Send(new UpdateAggregatedReviewRateDataCommand(Id), new CancellationToken());
        }

        public  Task<bool> UpdateAggregatedReviewRateDataAsync(Product product)//not necessary
        {
            throw new NotImplementedException();
        }

        public  Task<bool> UpdateAggregatedReviewRateDataAsync()//not necessary
        {
            throw new NotImplementedException();
        }
    }
}
