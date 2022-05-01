namespace PriceParser.Api.Models.ProductFromSite
{
    public class GetProductFromSiteInfoInProductModel
    {
        public Guid Id { get; set; }
        public string? SiteName { get; set; }
        public double Price { get; set; }
        public string? CurrencyCode { get; set; }
    }
}
