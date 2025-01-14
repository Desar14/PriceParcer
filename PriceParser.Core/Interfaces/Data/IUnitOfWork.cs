﻿using PriceParser.Core.Interfaces.Data;
using PriceParser.Data.Entities;

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
        IRepository<RefreshToken> RefreshTokens { get; }

        Task<int> Commit();
    }
}
