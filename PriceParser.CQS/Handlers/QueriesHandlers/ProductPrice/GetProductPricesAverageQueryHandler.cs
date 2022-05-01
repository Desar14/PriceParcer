using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PriceParser.Core.DTO;
using PriceParser.CQS.Models.Queries;
using PriceParser.Data;

namespace PriceParser.CQS.Handlers.QueriesHandlers
{
    public class GetProductPricesAverageQueryHandler : IRequestHandler<GetProductPricesAverageQuery, IEnumerable<ProductPriceDTO>>
    {
        protected readonly ApplicationDbContext _database;
        protected readonly IMapper _mapper;
        protected readonly ILogger<GetAllProductPricesQuery> _logger;
        protected readonly IMediator _mediator;

        public GetProductPricesAverageQueryHandler(ApplicationDbContext database, IMapper mapper, ILogger<GetAllProductPricesQuery> logger, IMediator mediator)
        {
            _database = database;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<IEnumerable<ProductPriceDTO>> Handle(GetProductPricesAverageQuery request, CancellationToken cancellationToken)
        {
            var prices = await _mediator.Send(new GetProductFromSitePricesQuery()
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                PerEveryDay = request.PerEveryDay,
                ProductFromSiteId = default,
                ProductId = request.ProductId
            }, new CancellationToken());

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

            return averageData;
        }
    }
}
