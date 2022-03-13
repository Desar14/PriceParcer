using Microsoft.AspNetCore.Identity;

namespace PriceParser.Models
{
    public class MarketSiteListItemViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }

        public DateTime Created { get; set; }
        public string? CreatedByUserName { get; set; }

        public bool IsAvailable { get; set; }
    }
}
