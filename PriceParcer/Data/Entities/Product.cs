namespace PriceParcer.Data
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public byte[] ImageData { get; set; }
        public bool UseExternalImage { get; set; }

        public bool Hidden { get; set; }  

        public virtual ProductInfoAggregated AggregatedInfo { get; set; }
        public virtual List<UserReview> Reviews { get; set; }
        public virtual List<ProductFromSites> FromSites { get; set; }

    }
}
