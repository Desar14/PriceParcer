namespace PriceParser.Models.ProductFromSite
{
    public class ProductFromSiteItemListViewModel
    {
        public Guid Id { get; set; }
        public string? ProductName { get; set; }
        public string? SiteName { get; set; }
        public bool DoNotParse { get; set; }        
        public DateTime Created { get; set; }
        public string? CreatedByUserName { get; set; }
    }
}
