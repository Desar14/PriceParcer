using AutoMapper;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PriceParser.Core.DTO;
using PriceParser.Core.Interfaces;
using PriceParser.Data;
using PriceParser.Data.Entities;


namespace PriceParser.Domain
{
    public class CurrenciesService : ICurrenciesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CurrenciesService> _logger;
        private readonly IConfiguration _configuration;

        public CurrenciesService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CurrenciesService> logger, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<bool> AddAsync(CurrencyDTO currencyDTO)
        {
            var entity = _mapper.Map<Currency>(currencyDTO);

            await _unitOfWork.Currencies.Add(entity);

            var result = await _unitOfWork.Commit();

            return result > 0;
        }

        public async Task<bool> AddFromNBRBAsync()
        {

            HttpResponseMessage? response;

            using (var client = new HttpClient())
            {
                try
                {
                    response = await client.GetAsync("https://www.nbrb.by/api/exrates/currencies");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,ex.Message);
                    throw;
                }
                
            }

            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                List<Currency>? currList;

                try
                {
                    currList = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Currency>>(response.Content.ReadAsStream());
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    throw;
                }                

                if (currList != null)
                {

                    List<Currency> currListToAdd = new();

                    foreach (var item in currList)
                    {
                        var exists = (await _unitOfWork.Currencies.FindBy(curr => curr.Cur_Code == item.Cur_Code)) != null;

                        if (!exists && item.Cur_DateStart <= DateTime.Now && item.Cur_DateEnd >= DateTime.Now)
                        {
                            currListToAdd.Add(item);
                        }
                    }
                    
                    
                    await _unitOfWork.Currencies.AddRange(currListToAdd);
                }
            }

            return await _unitOfWork.Commit() > 0;
        }

        public async Task<IEnumerable<ProductPriceDTO>> ConvertAtTheRate(IEnumerable<ProductPriceDTO> prices, Guid newCurrencyId, Guid? oldCurrency = null)
        {

            var currencyRates = await _unitOfWork.CurrencyRates.Get(currRate => currRate.CurrencyId == newCurrencyId, null, currRate => currRate.Currency);


            var result = prices.Join(
                currencyRates,
                t1 => new {t1.ParseDate.Date },
                t2 => new {t2.Date.Date },
                (t1, t2) => new ProductPriceDTO()
                {
                    Id = t1.Id,
                    ProductFromSiteId = t1.ProductFromSiteId,
                    ParseDate = t1.ParseDate,
                    FullPrice = Math.Round(t1.FullPrice/(double)(t2.Cur_OfficialRate ?? 1) * t2.Cur_Scale, 2),
                    DiscountPrice = t1.DiscountPrice,
                    DiscountPercent = t1.DiscountPercent,
                    CurrencyCode = t2.Currency.Cur_Abbreviation,
                    CurrencyId = t2.CurrencyId,
                    IsOutOfStock = t1.IsOutOfStock,
                    ParseError = t1.ParseError
                });

            return result;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await _unitOfWork.Currencies.Delete(id);

            var result = await _unitOfWork.Commit();

            return result > 0;
        }

        public async Task<bool> EditAsync(CurrencyDTO currencyDTO)
        {
            var entity = _mapper.Map<Currency>(currencyDTO);

            await _unitOfWork.Currencies.Update(entity);

            var result = await _unitOfWork.Commit();

            return result > 0;
        }

        public async Task<IEnumerable<CurrencyDTO>> GetAllAsync()
        {
            return (await _unitOfWork.Currencies.Get())
                .Select(record => _mapper.Map<CurrencyDTO>(record));
        }

        public async Task<CurrencyDTO> GetByAbbreviationAsync(string abbr)
        {
            var result = await _unitOfWork.Currencies.FindBy(item => item.Cur_Abbreviation == abbr);

            return _mapper.Map<CurrencyDTO>(result);
        }

        public async Task<CurrencyDTO> GetDetailsAsync(Guid id)
        {
            var result = await _unitOfWork.Currencies.GetByID(id);

            return _mapper.Map<CurrencyDTO>(result);
        }

        public async Task<IEnumerable<CurrencyRatesDTO>> GetRatesAsync(Guid CurrencyId)
        {
            return (await _unitOfWork.CurrencyRates.Get(x => x.CurrencyId == CurrencyId, x => x.OrderByDescending(x => x.Date)))
                .Select(record => _mapper.Map<CurrencyRatesDTO>(record));
        }

        public async Task<IEnumerable<CurrencyDTO>> GetUsableAsync()
        {
            var currList = (await _unitOfWork.Currencies.Get(curr => curr.AvailableForUsers == true)).Select(record => _mapper.Map<CurrencyDTO>(record));

            return currList;
        }

        public async Task<bool> ToggleUpdateRatesAsync(Guid Id, bool newStateUpdateRates, bool newStateAvailable)
        {

            var patchModel = new List<PatchModel>();

            patchModel.Add(new PatchModel()
            {
                PropertyName = "UpdateRates",
                PropertyValue = newStateUpdateRates
            });

            patchModel.Add(new PatchModel()
            {
                PropertyName = "AvailableForUsers",
                PropertyValue = newStateAvailable
            });

            await _unitOfWork.Currencies.PatchAsync(Id, patchModel);

            return (await _unitOfWork.Commit()) > 0;

        }

        public async Task<bool> UpdateRatesAsync()
        {

            var currList = (await _unitOfWork.Currencies.Get(curr => curr.UpdateRates == true));

            foreach (var item in currList)
            {
                await UpdateRatesAsync(item);
            }

            return true;
        }

        public async Task<bool> UpdateRatesAsync(Guid CurrencyId)
        {
            var currency = await _unitOfWork.Currencies.GetByID(CurrencyId);

            if (currency == null)
            {
                _logger.LogWarning($"Update rates: Currency with id {CurrencyId} not found in DB");
                return false;
            }

            return await UpdateRatesAsync(currency);
        }

        public async Task<bool> UpdateRatesAsync(Currency currency)
        {
            HttpResponseMessage response;
            var defaultCurrAbbr = _configuration["DefaultCurrency"];

            if (!int.TryParse(_configuration["CurrencyRatesMinimumHistoryDays"], out int minimumHistory))
                minimumHistory = 40;

            var lastDate = _unitOfWork.CurrencyRates.LastCurrencyRateDate(currency.Id);

            if (DateTime.Now - lastDate > TimeSpan.FromDays(minimumHistory))
            {
                lastDate = DateTime.Now.AddDays(-1 * minimumHistory);
            }

            if (DateTime.Now.Date == lastDate.Date)
            {
                return true;
            }

            if (currency.Cur_Abbreviation == defaultCurrAbbr)
            {
                List<CurrencyRate>? defaultCurrList = new();
                for (DateTime i = lastDate; i <= DateTime.Now.Date; i = i.AddDays(1))
                {
                    defaultCurrList.Add(new CurrencyRate()
                    {
                        Date = i.Date,
                        CurrencyId = currency.Id,
                        Cur_OfficialRate = 1,
                        Cur_Scale = 1,
                        Id = Guid.NewGuid()
                    });
                }
                await _unitOfWork.CurrencyRates.AddRange(defaultCurrList);
                return await _unitOfWork.Commit() > 0;
            }
            

            string url = $"https://www.nbrb.by/api/exrates/rates/dynamics/{currency.Cur_ID}";
            var param = new Dictionary<string, string>
            {
                { "startdate", lastDate.ToString("yyyy-MM-dd") },
                { "enddate", DateTime.Now.ToString("yyyy-MM-dd") }
            };

            var newUrl = new Uri(QueryHelpers.AddQueryString(url, param));


            using (var client = new HttpClient())
            {
                try
                {
                    response = await client.GetAsync(newUrl);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    throw;
                }
            }

            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                List<CurrencyRate>? currList;

                try
                {
                    currList = await System.Text.Json.JsonSerializer.DeserializeAsync<List<CurrencyRate>>(response.Content.ReadAsStream());
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    throw;
                }

                if (currList != null)
                {
                    currList.ForEach(currencyRate =>
                    {
                        currencyRate.CurrencyId = currency.Id;
                        currencyRate.Cur_Scale = currency.Cur_Scale;
                    });

                    await _unitOfWork.CurrencyRates.AddRange(currList);
                }
            }

            return await _unitOfWork.Commit() > 0;
        }
    }
}
