namespace PriceParser.Core.DTO
{
    public class CurrencyRatesDTO
    {
        public Guid id { get; set; }
        public CurrencyDTO Currency { get; set; }
        public Guid CurrencyId { get; set; }
        public DateTime Date { get; set; }
        public int Cur_Scale { get; set; }
        public decimal? Cur_OfficialRate { get; set; }
    }
}
