using Microsoft.AspNetCore.Identity;
using PriceParser.Data.Entities;

namespace PriceParser.Data
{
    public class UserReview : BaseEntity
    {
        public IdentityUser User { get; set; }
        public string UserId { get; set; }
        public Product Product { get; set; }
        public Guid ProductId { get; set; }
        public string ReviewTitle { get; set; }
        public string ReviewText { get; set; }
        public int ReviewScore { get; set; }
        public DateTime ReviewDate { get; set; }
        public bool Hidden { get; set; }

    }
}
