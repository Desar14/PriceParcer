using PriceParser.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PriceParser.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace PriceParser
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {

        public DbSet<MarketSite> MarketSites { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductFromSites> ProductsFromSites { get; set; }
        public DbSet<ProductPrice> ProductPricesHistory { get; set; }
        public DbSet<UserReview> UserReviews { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyRate> CurrencyRates { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
    }
}