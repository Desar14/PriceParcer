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

        public async Task<bool> ToggleUpdateRatesAsync(Guid Id, bool newState)
        {

            var patchModel = new List<PatchModel>();

            patchModel.Add(new PatchModel()
            {
                PropertyName = "UpdateRates",
                PropertyValue = newState
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
