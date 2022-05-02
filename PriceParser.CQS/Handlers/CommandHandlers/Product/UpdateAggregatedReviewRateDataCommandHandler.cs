using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using PriceParser.CQS.Models.Commands;
using PriceParser.Data;

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
            var aggReviewRateAverage = await _database.UserReviews
                .Where(x => x.ProductId == request.Id)
                .Select(x => x.ReviewScore).DefaultIfEmpty().AverageAsync(cancellationToken);

            var productEntity = await _database.Products.FirstOrDefaultAsync(x => x.Id == request.Id , cancellationToken: cancellationToken);

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
