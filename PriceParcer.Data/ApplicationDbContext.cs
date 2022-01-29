using PriceParcer.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PriceParcer
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public DbSet<MarketSite> MarketSites { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductFromSites> ProductsFromSites { get; set; }
        public DbSet<ProductPrice> ProductPricesHistory { get; set; }
        public DbSet<UserReview> UserReviews { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
    }
}