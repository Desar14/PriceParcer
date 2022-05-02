using PriceParser.Data.Entities;

namespace PriceParser.Core.Interfaces.Data
{
    public interface ICurrencyRatesRepository : IRepository<CurrencyRate>
    {
        public DateTime LastCurrencyRateDate(Guid CurrencyId);
    }
}
