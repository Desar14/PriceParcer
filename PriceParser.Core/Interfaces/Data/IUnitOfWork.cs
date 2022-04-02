using Microsoft.AspNetCore.Identity;
using PriceParser.Core.Interfaces.Data;
using PriceParser.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceParser.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<MarketSite> MarketSites { get; }
        IRepository<Product> Products { get; }  
        IRepository<ProductFromSites> ProductsFromSites { get; }
        IRepository<ProductPrice> ProductPricesHistory { get; }
        IRepository<UserReview> UserReviews { get; }
        IRepository<Currency> Currencies { get; }
        ICurrencyRatesRepository CurrencyRates { get; }

        Task<int> Commit();
    }
}
