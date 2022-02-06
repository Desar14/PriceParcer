namespace PriceParcer.Models
{
    public class MarketSiteInProductViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }  
        public double Price { get; set; }
        public string? CurrencyCode { get; set; }
    }
}
