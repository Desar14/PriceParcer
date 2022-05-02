using Microsoft.AspNetCore.Mvc.Rendering;
using PriceParser.Data.Entities;

namespace PriceParser.Models.UserReview
{
    public class UserReviewCreateEditViewModel
    {
        public Guid Id { get; set; }
        public ApplicationUser User { get; set; }
        public Guid UserId { get; set; }
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
