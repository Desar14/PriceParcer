using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PriceParser.CQS.Models.Commands;

namespace PriceParser.CQS.Handlers.CommandHandlers
{
    public class UpdateAggregatedReviewRateDataCommandHandler : IRequestHandler<UpdateAggregatedReviewRateDataCommand, bool>
    {
        protected readonly ApplicationDbContext _database;
        protected readonly IMapper _mapper;
        protected readonly ILogger<UpdateAggregatedPricesDataCommandHandler> _logger;

        public UpdateAggregatedReviewRateDataCommandHandler(ApplicationDbContext database, IMapper mapper, ILogger<UpdateAggregatedPricesDataCommandHandler> logger)
        {
            _database = database;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateAggregatedReviewRateDataCommand request, CancellationToken cancellationToken)
        {
            var aggReviewRateAverage = _database.UserReviews
                .Where(x => x.ProductId == request.Id)
                .Select(x => x.ReviewScore).DefaultIfEmpty().Average();

            var productEntity = await _database.Products.FindAsync(request.Id, cancellationToken);

            if (productEntity != null)
            {
                productEntity.AverageScore = (float)aggReviewRateAverage;

                _database.Products.Update(productEntity);
            }

            var result = await _database.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
