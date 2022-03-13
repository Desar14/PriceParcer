using PriceParser.Data.Entities;

namespace PriceParser.Data
{
    public class ProductPrice : BaseEntity
    {
        public virtual ProductFromSites ProductFromSite { get; set; }
        public Guid ProductFromSiteId { get; set; }

        public DateTime ParseDate { get; set; }
        public double FullPrice { get; set; }
        public double DiscountPrice { get; set; }
        public double DiscountPercent { get; set; }
        public string CurrencyCode { get; set; }
        public bool IsOutOfStock { get; set; }


    }
}
