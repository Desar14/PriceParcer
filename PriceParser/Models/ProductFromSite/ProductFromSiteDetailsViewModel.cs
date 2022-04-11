using Microsoft.AspNetCore.Mvc.Rendering;
using PriceParser.Models.ProductPrice;

namespace PriceParser.Models.ProductFromSite
{
    public class ProductFromSiteDetailsViewModel
    {
        public Guid Id { get; set; }
        public string? ProductName { get; set; }
        public string? SiteName { get; set; }
        public string? Path { get; set; }
        public bool DoNotParse { get; set; }
        public string? ParseSchedule { get; set; }
        public DateTime Created { get; set; }
        public string? CreatedByUserName { get; set; }

        public List<ProductPriceItemListViewModel> productPrices { get; set; }
        public List<SelectListItem> Currencies { get; set; }
    }
}
