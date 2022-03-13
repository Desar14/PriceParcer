namespace PriceParser.Models
{
    public class ProductDeleteViewModel
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
    }
}
