namespace PriceParcer.Data
{
    public class ProductPrice
    {
        public Guid Id { get; set; }

        public virtual ProductFromSites Product { get; set; }
        public Guid ProductId { get; set; }

        public DateTime ParseDate { get; set; }
        public double FullPrice { get; set; }
        public double DiscountPrice { get; set; }
        public double DiscountPercent { get; set; }
        public string CurrencyCode { get; set; }


    }
}
