namespace PriceParcer.Models
{
    public class UserReviewInProductViewModel
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string? ReviewText { get; set; }
        public int ReviewScore { get; set; }
    }
}
