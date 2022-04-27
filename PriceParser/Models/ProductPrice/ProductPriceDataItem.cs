
using System.Text.Json.Serialization;

namespace PriceParser.Models.ProductPrice
{
    public class ProductPriceDataItem
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        [JsonPropertyName("price")]
        public double Price { get; set; }
    }

}
