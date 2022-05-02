using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PriceParser.CQS.Models.Commands;

namespace PriceParser.CQS.Handlers.CommandHandlers
{
    public class UpdateAggregatedPricesDataCommandHandler : IRequestHandler<UpdateAggregatedPricesDataCommand, bool>
    {
        protected readonly ApplicationDbContext _database;
        protected readonly IMapper _mapper;
        protected readonly ILogger<UpdateAggregatedPricesDataCommandHandler> _logger;

        public UpdateAggregatedPricesDataCommandHandler(ApplicationDbContext database, IMapper mapper, ILogger<UpdateAggregatedPricesDataCommandHandler> logger)
        {
            _database = database;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateAggregatedPricesDataCommand request, CancellationToken cancellationToken)
        {
            var aggOverallData = await _database.ProductPricesHistory
                .Where(x => x.ProductFromSite.ProductId == request.Id && x.FullPrice != 0)
                .GroupBy(x => 1)
                .Select(x => new
                {
                    AveragePrice = x.Average(x => x.FullPrice),
                    BestPrice = x.Min(x => x.FullPrice)
                }).FirstOrDefaultAsync(cancellationToken);

            var aggNowData = await _database.ProductPricesHistory
                .Where(x => x.ProductFromSite.ProductId == request.Id && x.FullPrice != 0)
                .GroupBy(x => x.ProductFromSiteId, x => x.ParseDate, (prodId, date) => new
                {
                    ProdFromSiteId = prodId,
                    MaxDate = date.Max()
                })
                .Join(_database.ProductPricesHistory,
                        maxDates => new { q1 = maxDates.ProdFromSiteId, q2 = maxDates.MaxDate },
                        rawTable => new { q1 = rawTable.ProductFromSiteId, q2 = rawTable.ParseDate },
                        (maxDates, rawTable) => new
                        {
                            maxDates.ProdFromSiteId,
                            CurrentPrice = rawTable.FullPrice
                        })
                .GroupBy(x => 1)
                .Select(x => new
                {
                    AveragePrice = x.Average(x => x.CurrentPrice),
                    BestPrice = x.Min(x => x.CurrentPrice)
                }).FirstOrDefaultAsync(cancellationToken);

            var productEntity = await _database.Products.FindAsync(request.Id);

            if (productEntity != null)
            {
                productEntity.AveragePriceOverall = aggOverallData.AveragePrice;
                productEntity.BestPriceOverall = aggOverallData.BestPrice;
                productEntity.AveragePriceNow = aggNowData.AveragePrice;
                productEntity.BestPriceNow = aggNowData.BestPrice;

                productEntity.LastAggregate = DateTime.Now;

                _database.Products.Update(productEntity);
            }

            var result = await _database.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
