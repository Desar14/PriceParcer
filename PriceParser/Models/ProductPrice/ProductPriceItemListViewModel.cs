namespace PriceParser.Models.ProductPrice
{
    public class ProductPriceItemListViewModel
    {
        public Guid Id { get; set; }
        public DateTime ParseDate { get; set; }
        public double FullPrice { get; set; }
        public double DiscountPrice { get; set; }
        public double DiscountPercent { get; set; }
        public string CurrencyCode { get; set; }
        public bool IsOutOfStock { get; set; }
        public bool ParseError { get; set; }
    }
}
