using Microsoft.AspNetCore.Identity;
using PriceParcer.Data.Entities;

namespace PriceParcer.Data
{
    public class MarketSite : BaseEntity
    {
        public string? Name { get; set; }
        public string? AuthType { get; set; }
        public string? SiteLogin { get; set; }
        public string? SitePassword { get; set; }        
        public ParseTypes? ParseType { get; set; }
        public string? ParsePricePath { get; set; }
        public string? ParseCurrencyPath { get; set; }

        public bool IsAvailable { get; set; }
        public DateTime Created { get; set; }
        public virtual IdentityUser? CreatedByUser { get; set; }
        public string? CreatedByUserId { get; set; }

        public virtual List<ProductFromSites>? Products { get; set; }

    }

    public enum ParseTypes
    {
        Xpath,
        Other
    }
}
