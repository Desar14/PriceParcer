using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PriceParser.Core.DTO;
using PriceParser.Core.Utils;
using PriceParser.CQS.Models.Queries;
using PriceParser.Data;
using PriceParser.Data.Entities;
using System.Linq.Expressions;

namespace PriceParser.CQS.Handlers.QueriesHandlers
{
    public class GetProductFromSitePricesQueryHandler : IRequestHandler<GetProductFromSitePricesQuery, IEnumerable<ProductPriceDTO>>
    {
        protected readonly ApplicationDbContext _database;
        protected readonly IMapper _mapper;
        protected readonly ILogger<GetAllProductPricesQuery> _logger;

        public GetProductFromSitePricesQueryHandler(ApplicationDbContext database, IMapper mapper, ILogger<GetAllProductPricesQuery> logger)
        {
            _database = database;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductPriceDTO>> Handle(GetProductFromSitePricesQuery request, CancellationToken cancellationToken)
        {
            if (request.StartDate == null)
            {
                request.StartDate = DateTime.MinValue;
                request.PerEveryDay = false;
            }

            if (request.EndDate == null)
            {
                request.EndDate = DateTime.MinValue;
                request.PerEveryDay = false;
            }

            //start of day and end of day
            request.StartDate = request.StartDate?.Date;
            request.EndDate = request.EndDate?.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            Expression<Func<ProductPrice, bool>>? filter;

            if (request.ProductId != default && request.ProductFromSiteId == default)
            {
                filter = price =>
                    price.ProductFromSite.ProductId == request.ProductId
                    && price.ParseDate >= request.StartDate
                    && price.ParseDate <= request.EndDate
                    && !price.ParseError;
            }
            else if (request.ProductFromSiteId != default && request.ProductId == default)
            {
                filter = price =>
                    price.ProductFromSiteId == request.ProductFromSiteId
                    && price.ParseDate >= request.StartDate
                    && price.ParseDate <= request.EndDate
                    && !price.ParseError;
            }
            else
                throw new ArgumentException("Incorrect id arguments!");

            var prices = _database.ProductPricesHistory.Where(filter).Include(price => price.ProductFromSite).Select(price => _mapper.Map<ProductPriceDTO>(price));


            if (request.PerEveryDay && request.StartDate != null && request.EndDate != null)
            {
                return PricesProcessing.MakePricesPerEveryDay(prices, request.StartDate.Value, request.EndDate.Value);
            }
            return prices;
        }
    }
}
