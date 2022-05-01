using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PriceParser.Api.Models.Product
{
    public class PostPutProductModel
    {
        [JsonIgnore]
        public Guid Id = Guid.NewGuid();
        [Required]
        public string? Name { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
        public bool Hidden { get; set; }
        [Required]
        public string? CurrencyCode { get; set; }
    }
}
