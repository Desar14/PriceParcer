using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PriceParcer.Models.ProductFromSite
{
    public class ProductFromSiteCreateEditViewModel
    {
        public Guid Id { get; set; }
        public ProductItemListViewModel? Product { get; set; }
        public Guid ProductId { get; set; }

        public MarketSiteListItemViewModel? Site { get; set; }
        public Guid SiteId { get; set; }

        public string? Path { get; set; }
        public bool DoNotParse { get; set; }

        public string? ParseSchedule { get; set; }

        public DateTime Created { get; set; }
        public virtual IdentityUser? CreatedByUser { get; set; }
        public string? CreatedByUserId { get; set; }

        public List<SelectListItem> UsersList { get; set; }
        public List<SelectListItem> ProductsList { get; set; }
        public List<SelectListItem> SitesList { get; set; }

    }
}
