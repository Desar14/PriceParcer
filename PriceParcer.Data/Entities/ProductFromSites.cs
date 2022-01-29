using Microsoft.AspNetCore.Identity;
using PriceParcer.Data.Entities;

namespace PriceParcer.Data
{
    public class ProductFromSites : BaseEntity
    {
        public virtual Product product { get; set; }
        public Guid ProductId { get; set; }

        public virtual MarketSite Site { get; set; }
        public Guid SiteId { get; set; } 

        public string? Path { get; set; }
        public bool DoNotParse { get; set; }

        public string? ParseSchedule { get; set; }

        public DateTime Created { get; set; }
        public IdentityUser? CreatedByUser { get; set; }
        public string? CreatedByUserId { get; set; }

        public virtual List<ProductPrice> Prices { get; set; }


    }
}
