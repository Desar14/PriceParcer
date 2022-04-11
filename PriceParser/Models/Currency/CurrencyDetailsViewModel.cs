using PriceParser.Models.Currency;

namespace PriceParser.Models
{
    public class CurrencyDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Abbreviation { get; set; }
        public string Name { get; set; }
        public int Scale { get; set; }
        public int Periodicity { get; set; }
        public bool UpdateRates { get; set; }
        public bool AvailableForUsers { get; set; }
        public IEnumerable<CurrencyRateListItemModel> Rates { get; set; }
    }
}
