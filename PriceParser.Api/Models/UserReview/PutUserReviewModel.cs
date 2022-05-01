using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PriceParser.Api.Models.UserReview
{
    public class PutUserReviewModel
    {
        public string UserId { get; set; }
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public string ReviewTitle { get; set; }
        [Required]
        public string ReviewText { get; set; }
        public int ReviewScore { get; set; }
        public DateTime ReviewDate { get; set; }
        public bool Hidden { get; set; }
    }
}
