namespace PriceParcer.Models
{
    public class CreateEditProductViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public byte[]? ImageData { get; set; }
        public bool UseExternalImage { get; set; }

        public bool Hidden { get; set; }

        //todo delete this from create
        public double BestPriceNow { get; set; }
        public double BestPriceOverall { get; set; }

        public double AveragePriceNow { get; set; }
        public double AveragePriceOverall { get; set; }
        public string? CurrencyCode { get; set; }

        public float AverageScore { get; set; }
    }
}
