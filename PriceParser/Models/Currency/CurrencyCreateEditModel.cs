namespace PriceParser.Models.Currency
{
    public class CurrencyToggleUpdatingRatesModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Abbreviation { get; set; }
        public string Name { get; set; }
        public int Scale { get; set; }
        public int Periodicity { get; set; }
        public bool UpdateRates { get; set; }
    }
}
