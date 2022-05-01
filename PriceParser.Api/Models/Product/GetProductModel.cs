using PriceParser.Api.Models.ProductFromSite;
using PriceParser.Api.Models.UserReview;

namespace PriceParser.Api.Models.Product
{
    public class GetProductModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
       
        public bool Hidden { get; set; }

        public double BestPriceNow { get; set; }
        public double BestPriceOverall { get; set; }

        public double AveragePriceNow { get; set; }
        public double AveragePriceOverall { get; set; }
        public string? CurrencyCode { get; set; }

        public float AverageScore { get; set; }

        public List<GetUserReviewInProductModel> Reviews { get; set; }
        public List<GetProductFromSiteInfoInProductModel> FromSites { get; set; }
    }
}
