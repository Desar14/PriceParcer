using AutoMapper;
using MediatR;
using PriceParser.CQS.Models.Commands;
using PriceParser.Data;
using PriceParser.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceParser.CQS.Handlers.CommandHandlers
{
    public class AddProductPriceRangeCommandHandler : IRequestHandler<AddProductPriceRangeCommand, bool>
    {
        private readonly ApplicationDbContext _database;
        private readonly IMapper _mapper;

        public AddProductPriceRangeCommandHandler(ApplicationDbContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<bool> Handle(AddProductPriceRangeCommand command, CancellationToken token)
        {
            var entityRange = command.productPriceDTOs.Select(x => _mapper.Map<ProductPrice>(x));

            var lastPricesPerProdFromSiteId = _database.ProductPricesHistory
                .Where(x => x.FullPrice != 0)
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
                    });

            var entityRangeFiltered = entityRange.Where(x => !lastPricesPerProdFromSiteId.Any(y => x.ProductFromSiteId == y.ProdFromSiteId && x.FullPrice == y.CurrentPrice));

            await _database.ProductPricesHistory.AddRangeAsync(entityRangeFiltered, token);

            var result = await _database.SaveChangesAsync(token);

            return result > 0;
        }
    }
}
