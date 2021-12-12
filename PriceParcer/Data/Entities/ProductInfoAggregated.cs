namespace PriceParcer.Data
{
    public class ProductInfoAggregated
    {
        public Guid Id { get; set; }
        public virtual Product Product { get; set; }
        public Guid ProductId { get; set; }

        public DateTime LastAggregate { get; set; }

        public double BestPriceNow { get; set; }
        public double BestPriceOverall { get; set; }

        public double AveragePriceNow { get; set; }
        public double AveragePriceOverall { get; set; }
        public string CurrencyCode { get; set; }

        public float AverageScore { get; set; }

    }
}
