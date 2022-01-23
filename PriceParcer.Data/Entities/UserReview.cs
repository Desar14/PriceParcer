using Microsoft.AspNetCore.Identity;
using PriceParcer.Data.Entities;

namespace PriceParcer.Data
{
    public class UserReview : BaseEntity
    {
        public IdentityUser User { get; set; }
        public string UserId { get; set; }
        public Product Product { get; set; }
        public Guid ProductId { get; set; }

        public string ReviewText { get; set; }
        public int ReviewScore { get; set; }

        public bool Hidden { get; set; }

    }
}
