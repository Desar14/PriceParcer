using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace PriceParser.Data.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public virtual Currency? UserCurrency { get; set; }
        public Guid? UserCurrencyId { get; set; }
        [JsonIgnore]
        public virtual List<RefreshToken> RefreshTokens { get; set; }
    }
}
