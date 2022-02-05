using Microsoft.AspNetCore.Identity;

namespace PriceParcer.Models
{
    public class MarketSiteListItemViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }

        public DateTime Created { get; set; }
        public string? CreatedByUserName { get; set; }
    }
}
