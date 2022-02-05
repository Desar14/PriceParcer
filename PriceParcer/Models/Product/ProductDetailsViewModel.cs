namespace PriceParcer.Models
{
    public class ProductDetailsViewModel
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
           
        public double BestPriceNow { get; set; }
        public double BestPriceOverall { get; set; }

        public double AveragePriceNow { get; set; }
        public double AveragePriceOverall { get; set; }
        public string? CurrencyCode { get; set; }

        public float AverageScore { get; set; }

        public List<MarketSiteInProductViewModel> marketSites { get; set; } 

        public List<UserReviewInProductViewModel> userReviews { get; set; } 
    }
}
