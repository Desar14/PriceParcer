namespace PriceParcer.Data
{
    public class ProductPrice
    {
        public Guid Id { get; set; }

        public virtual ProductFromSites ProductFromSite { get; set; }
        public Guid ProductFromSiteId { get; set; }

        public DateTime ParseDate { get; set; }
        public double FullPrice { get; set; }
        public double DiscountPrice { get; set; }
        public double DiscountPercent { get; set; }
        public string CurrencyCode { get; set; }


    }
}
