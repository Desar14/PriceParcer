using PriceParser.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceParser.Core.Interfaces.Data
{
    public interface ICurrencyRatesRepository : IRepository<CurrencyRate>
    {
        public DateTime LastCurrencyRateDate(Guid CurrencyId);
    }
}
