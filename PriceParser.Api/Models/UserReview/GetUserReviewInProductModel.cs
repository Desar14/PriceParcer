namespace PriceParser.Api.Models.UserReview
{
    public class GetUserReviewInProductModel
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string? ReviewTitle { get; set; }
        public string? ReviewText { get; set; }
        public int ReviewScore { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}
