using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PriceParser.Core.DTO;
using PriceParser.CQS.Handlers.CommandHandlers;
using PriceParser.CQS.Models.Queries;

namespace PriceParser.CQS.Handlers.QueriesHandlers
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDTO>>
    {
        protected readonly ApplicationDbContext _database;
        protected readonly IMapper _mapper;
        protected readonly ILogger<EditProductCommandHandler> _logger;

        public GetAllProductsQueryHandler(ApplicationDbContext database, IMapper mapper, ILogger<EditProductCommandHandler> logger)
        {
            _database = database;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductDTO>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            if (request.PageNumber != 0)
            {
                return await _database.Products
                    .Include(x => x.FromSites).ThenInclude(x => x.Site)
                    .Include(x => x.Reviews).ThenInclude(x => x.User)
                    .Skip(request.PageSize * request.PageNumber).Take(request.PageSize)
                    .Select(product => _mapper.Map<ProductDTO>(product)).ToListAsync(cancellationToken);
            }
            else
                return await _database.Products
                    .Include(x => x.FromSites).ThenInclude(x => x.Site)
                    .Include(x => x.Reviews).ThenInclude(x => x.User)
                    .Select(product => _mapper.Map<ProductDTO>(product)).ToListAsync(cancellationToken);

        }
    }
}
