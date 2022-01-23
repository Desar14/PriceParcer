using PriceParcer.Core;
using PriceParcer.Core.Interfaces;
using PriceParcer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceParcer.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
        }

        public IRepository<MarketSite> MarketSites => new Repository<MarketSite>(_db);

        public IRepository<Product> Products => new Repository<Product>(_db);

        public IRepository<ProductFromSites> ProductsFromSites => new Repository<ProductFromSites>(_db);

        public IRepository<ProductPrice> ProductPricesHistory => new Repository<ProductPrice>(_db);

        public IRepository<UserReview> UserReviews => new Repository<UserReview>(_db);

        public async Task<int> Commit()
        {
            return await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db?.Dispose();

            MarketSites.Dispose();
            Products.Dispose();
            ProductsFromSites.Dispose();
            ProductPricesHistory.Dispose();
            UserReviews.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
