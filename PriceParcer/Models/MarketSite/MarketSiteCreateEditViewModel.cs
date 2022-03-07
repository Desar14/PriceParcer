using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using PriceParcer.Data;

namespace PriceParcer.Models
{
    public class MarketSiteCreateEditViewModel
    {
        public Guid Id { get; set; }
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
        public IdentityUser? CreatedByUser { get; set; }
        public string? CreatedByUserId { get; set; }

        public List<SelectListItem> UsersList { get; set; }

        public List<SelectListItem> ParseTypesList { get; set; }
    }
}
