namespace PriceParser.Data.Entities
{
    public class CurrencyRate : BaseEntity
    {
        public Currency Currency { get; set; }
        public Guid CurrencyId { get; set; }
        public DateTime Date { get; set; }
        public int Cur_Scale { get; set; }
        public decimal? Cur_OfficialRate { get; set; }
    }
    
}
