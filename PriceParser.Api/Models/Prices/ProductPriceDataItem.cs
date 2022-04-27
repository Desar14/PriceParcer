using System.Text.Json.Serialization;

namespace PriceParser.Api.Models.Prices
{
    public class ProductPriceDataItem
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        [JsonPropertyName("price")]
        public double Price { get; set; }
    }
}
