using PriceParser.Core.DTO;
using PriceParser.Data.Entities;

namespace PriceParser.Core.Interfaces
{
    public interface ICurrenciesService
    {
        Task<IEnumerable<CurrencyDTO>> GetAllAsync();
        Task<IEnumerable<CurrencyDTO>> GetUsableAsync();
        Task<CurrencyDTO> GetDetailsAsync(Guid id);
        Task<CurrencyDTO> GetByAbbreviationAsync(string abbr);

        Task<bool> AddAsync(CurrencyDTO currencyDTO);
        Task<bool> AddFromNBRBAsync();

        Task<bool> EditAsync(CurrencyDTO currencyDTO);
        Task<bool> ToggleUpdateRatesAsync(Guid Id, bool newStateUpdateRates, bool newStateAvailable);

        Task<bool> DeleteAsync(Guid id);

        Task<IEnumerable<CurrencyRatesDTO>> GetRatesAsync(Guid CurrencyId);

        Task<bool> UpdateRatesAsync();
        Task<bool> UpdateRatesAsync(Guid CurrencyId);
        Task<bool> UpdateRatesAsync(Currency currency);

        Task<IEnumerable<ProductPriceDTO>> ConvertAtTheRate(IEnumerable<ProductPriceDTO> prices, Guid newCurrencyId, Guid? oldCurrency = null);
    }
}
