namespace PriceParser.Models.UserReview
{
    public class UserReviewItemListViewModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }        
        public string ProductName { get; set; }
        public string ReviewTitle { get; set; }
        public int ReviewScore { get; set; }
        public DateTime ReviewDate { get; set; }
        public bool Hidden { get; set; }
    }
}
