using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PriceParser.Core.DTO;
using PriceParser.CQS.Models.Queries;

namespace PriceParser.CQS.Handlers.QueriesHandlers
{
    public class GetProductDetailsQueryHandler : IRequestHandler<GetProductDetailsQuery, ProductDTO>
    {
        protected readonly ApplicationDbContext _database;
        protected readonly IMapper _mapper;
        protected readonly ILogger<GetProductDetailsQuery> _logger;

        public GetProductDetailsQueryHandler(ApplicationDbContext database, IMapper mapper, ILogger<GetProductDetailsQuery> logger)
        {
            _database = database;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ProductDTO> Handle(GetProductDetailsQuery request, CancellationToken cancellationToken)
        {
            var result = await _database.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (result != null)
            {
                result.FromSites = await _database.ProductsFromSites.Where(prod => prod.ProductId == request.Id).Include(prod => prod.Site).ToListAsync();
                result.Reviews = await _database.UserReviews.Where(prod => prod.ProductId == request.Id).Include(prod => prod.User).ToListAsync();
            }

            return _mapper.Map<ProductDTO>(result);
        }
    }
}
