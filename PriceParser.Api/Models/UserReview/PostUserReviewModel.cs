using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PriceParser.Api.Models.UserReview
{
    public class PostUserReviewModel
    {
        public Guid Id = Guid.NewGuid();
        [JsonIgnore,ValidateNever]
        public string UserId { get; set; }
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public string ReviewTitle { get; set; }
        [Required]
        public string ReviewText { get; set; }
        public int ReviewScore { get; set; }
        public DateTime ReviewDate = DateTime.Now;
        public bool Hidden = false;
    }
}
