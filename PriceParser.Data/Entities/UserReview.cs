namespace PriceParser.Data.Entities
{
    public class UserReview : BaseEntity
    {
        public ApplicationUser User { get; set; }
        public Guid UserId { get; set; }
        public Product Product { get; set; }
        public Guid ProductId { get; set; }
        public string ReviewTitle { get; set; }
        public string ReviewText { get; set; }
        public int ReviewScore { get; set; }
        public DateTime ReviewDate { get; set; }
        public bool Hidden { get; set; }

    }
}
