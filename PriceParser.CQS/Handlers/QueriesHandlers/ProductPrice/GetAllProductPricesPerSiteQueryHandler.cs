using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PriceParser.Core.DTO;
using PriceParser.CQS.Models.Queries;
using PriceParser.Data;

namespace PriceParser.CQS.Handlers.QueriesHandlers
{
    public class GetAllProductPricesPerSiteQueryHandler : IRequestHandler<GetAllProductPricesPerSiteQuery, IEnumerable<ProductFromSitesDTO>>
    {
        protected readonly ApplicationDbContext _database;
        protected readonly IMapper _mapper;
        protected readonly ILogger<GetAllProductPricesQuery> _logger;
        protected readonly IMediator _mediator;

        public GetAllProductPricesPerSiteQueryHandler(ApplicationDbContext database, IMapper mapper, ILogger<GetAllProductPricesQuery> logger, IMediator mediator)
        {
            _database = database;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<IEnumerable<ProductFromSitesDTO>> Handle(GetAllProductPricesPerSiteQuery request, CancellationToken cancellationToken)
        {
            var prices = await _mediator.Send(new GetProductFromSitePricesQuery()
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                PerEveryDay = request.PerEveryDay,
                ProductFromSiteId = default,
                ProductId = request.ProductId
            }, new CancellationToken());

            var agg = prices.GroupBy(x => x.ProductFromSiteId).Select(x => new ProductFromSitesDTO
            {
                Id = x.Key,
                Prices = x.ToList()
            }).Join(
                _database.ProductsFromSites.Where(x => x.ProductId == request.ProductId).Include(x => x.Site),
                x => x.Id,
                y => y.Id,
                (x, y) => _mapper.Map<ProductFromSitesDTO>(y, opt => opt.AfterMap((src, dest) => dest.Prices = x.Prices))
                ).ToList();

            return agg;
        }
    }
}
