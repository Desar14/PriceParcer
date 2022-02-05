namespace PriceParcer.Models.ProductFromSite
{
    public class ProductFromSiteDetailsViewModel
    {
        public Guid Id { get; set; }
        public string? ProductName { get; set; }
        public string? SiteName { get; set; }
        public string? Path { get; set; }
        public bool DoNotParse { get; set; }
        public string? ParseSchedule { get; set; }
        public DateTime Created { get; set; }
        public string? CreatedByUserName { get; set; }
    }
}
