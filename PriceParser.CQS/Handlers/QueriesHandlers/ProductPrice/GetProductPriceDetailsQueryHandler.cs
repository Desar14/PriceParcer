using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PriceParser.Core.DTO;
using PriceParser.CQS.Models.Queries;
using PriceParser.Data;

namespace PriceParser.CQS.Handlers.QueriesHandlers
{
    public class GetProductPriceDetailsQueryHandler : IRequestHandler<GetProductPriceDetailsQuery, ProductPriceDTO>
    {
        protected readonly ApplicationDbContext _database;
        protected readonly IMapper _mapper;
        protected readonly ILogger<GetAllProductPricesQuery> _logger;

        public GetProductPriceDetailsQueryHandler(ApplicationDbContext database, IMapper mapper, ILogger<GetAllProductPricesQuery> logger)
        {
            _database = database;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ProductPriceDTO> Handle(GetProductPriceDetailsQuery request, CancellationToken cancellationToken)
        {
            var result = await _database.ProductPricesHistory.Where(x => x.Id == request.PriceId).Include(price => price.ProductFromSite).FirstOrDefaultAsync(cancellationToken);

            return _mapper.Map<ProductPriceDTO>(result);
        }
    }
}
