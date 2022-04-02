namespace PriceParser.Models.Currency
{
    public class CurrencyRateListItemModel
    {
        public DateTime Date { get; set; }
        public int Scale { get; set; }
        public decimal? OfficialRate { get; set; }
    }
}
