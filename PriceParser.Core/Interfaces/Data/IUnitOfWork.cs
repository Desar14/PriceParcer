using Microsoft.AspNetCore.Identity;
using PriceParser.Data;
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

        Task<int> Commit();
    }
}
