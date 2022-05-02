namespace PriceParser.Api.Models.Currency
{
    public class GetCurrencyModel
    {
        public Guid Id { get; set; }        
        public string Cur_Code { get; set; }
        public string Cur_Abbreviation { get; set; }
        public string Cur_Name { get; set; }
        public int Cur_Scale { get; set; }
        public int Cur_Periodicity { get; set; }

        public bool UpdateRates { get; set; }
        public bool AvailableForUsers { get; set; }
    }
}
