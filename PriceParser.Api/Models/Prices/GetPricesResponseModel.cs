using System.Text.Json.Serialization;

namespace PriceParser.Api.Models.Prices
{
    public class GetPricesResponseModel
    {
        [JsonPropertyName("product_from_site_id")]
        public Guid ProductFromSiteId { get; set; }

        [JsonPropertyName("site_name")]
        public string? SiteName { get; set; }

        [JsonPropertyName("currency")]
        public string? CurrencyCode { get; set; }

        [JsonPropertyName("prices")]
        public List<ProductPriceDataItem> Prices { get; set; } = new();

    }
}
