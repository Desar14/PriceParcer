using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PriceParcer.Models.UserReview
{
    public class UserReviewCreateEditViewModel
    {
        public Guid Id { get; set; }
        public IdentityUser User { get; set; }
        public string UserId { get; set; }
        public ProductItemListViewModel Product { get; set; }
        public Guid ProductId { get; set; }
        public string ReviewTitle { get; set; }
        public string ReviewText { get; set; }
        public int ReviewScore { get; set; }
        public DateTime ReviewDate { get; set; } 
        public bool Hidden { get; set; }

        public List<SelectListItem> UsersList { get; set; }
        public List<SelectListItem> ProductsList { get; set; }
    }
}
