using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PriceParser.Api.Models.Authentication
{
    public class RefreshTokenModel
    {
        [Required, JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
