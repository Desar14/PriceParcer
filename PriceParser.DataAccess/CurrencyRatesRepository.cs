using Microsoft.Extensions.Logging;
using PriceParser.Core.Interfaces.Data;
using PriceParser.Data.Entities;

namespace PriceParser.DataAccess
{
    public class CurrencyRatesRepository : Repository<CurrencyRate>, ICurrencyRatesRepository
    {
        public CurrencyRatesRepository(ApplicationDbContext context, ILogger<Repository<CurrencyRate>> logger) : base(context, logger)
        {
        }

        public DateTime LastCurrencyRateDate(Guid CurrencyId)
        {
            var result = _dbSet.Where(x => x.CurrencyId == CurrencyId).Select(x => x.Date).DefaultIfEmpty().Max();

            return result;
        }
    }
}
