using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PriceParser.Core.DTO;
using PriceParser.CQS.Models.Queries;

namespace PriceParser.CQS.Handlers.QueriesHandlers
{
    public class GetAllProductPricesQueryHandler : IRequestHandler<GetAllProductPricesQuery, IEnumerable<ProductPriceDTO>>
    {
        protected readonly ApplicationDbContext _database;
        protected readonly IMapper _mapper;
        protected readonly ILogger<GetAllProductPricesQuery> _logger;

        public GetAllProductPricesQueryHandler(ApplicationDbContext database, IMapper mapper, ILogger<GetAllProductPricesQuery> logger)
        {
            _database = database;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductPriceDTO>> Handle(GetAllProductPricesQuery request, CancellationToken cancellationToken)
        {
            return await _database.ProductPricesHistory.Where(price => price.ProductFromSiteId == request.ProductFromSitesId).Include(price => price.ProductFromSite)
                 .Select(price => _mapper.Map<ProductPriceDTO>(price)).ToListAsync(cancellationToken);
        }
    }
}
