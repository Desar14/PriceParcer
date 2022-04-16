using Microsoft.AspNetCore.Identity;
using PriceParser.Data.Entities;

namespace PriceParser.Data.Entities
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
        public string? ParsePriceAttributeName { get; set; }
        public string? ParseCurrencyAttributeName { get; set; }

        public bool IsAvailable { get; set; }
        public DateTime Created { get; set; }
        public virtual ApplicationUser? CreatedByUser { get; set; }
        public Guid? CreatedByUserId { get; set; }

        public virtual List<ProductFromSites>? Products { get; set; }

    }

    public enum ParseTypes
    {
        Xpath,
        Other
    }
}
