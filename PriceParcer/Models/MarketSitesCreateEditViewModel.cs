using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PriceParcer.Models
{
    public class MarketSitesCreateEditViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? AuthType { get; set; }
        public string? SiteLogin { get; set; }
        public string? SitePassword { get; set; }
        public string? ParseType { get; set; }

        public bool IsAvailable { get; set; }
        public DateTime Created { get; set; }
        public IdentityUser? CreatedByUser { get; set; }
        public string? CreatedByUserId { get; set; }

        public List<SelectListItem> UsersList { get; set; }
    }
}
