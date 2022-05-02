using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceParser.Core.Interfaces;
using PriceParser.Models;
using PriceParser.Models.Currency;

namespace PriceParser.Controllers
{
    [Authorize]
    public class CurrenciesController : Controller
    {

        private readonly ICurrenciesService _currenciesService;
        private readonly ILogger<CurrenciesController> _logger;
        private readonly IMapper _mapper;

        public CurrenciesController(ILogger<CurrenciesController> logger, ICurrenciesService currenciesService, IMapper mapper)
        {
            _logger = logger;
            _currenciesService = currenciesService;
            _mapper = mapper;
        }
        [AllowAnonymous]
        // GET: CurrenciesController
        public async Task<IActionResult> Index()
        {
            try
            {
                var currencies = await _currenciesService.GetAllAsync();

                var model = _mapper.Map<IEnumerable<CurrencyListItemViewModel>>(currencies).OrderBy(item => item.Code);

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                throw;
            }
        }

        // GET: CurrenciesController/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var dto = await _currenciesService.GetDetailsAsync(id);

                var rates = (await _currenciesService.GetRatesAsync(id)).Select(x => _mapper.Map<CurrencyRateListItemModel>(x));

                var model = _mapper.Map<CurrencyDetailsViewModel>(dto);
                model.Rates = rates;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateRates(Guid currencyId)
        {
            await _currenciesService.UpdateRatesAsync(currencyId);

            return RedirectToAction(nameof(Details), new { Id = currencyId });
        }

        // GET: CurrenciesController/Create
        public async Task<IActionResult> Create()
        {
            return NotFound();
        }

        // POST: CurrenciesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection)
        {

            return NotFound();

            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CurrenciesController/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var dto = (await _currenciesService.GetDetailsAsync(id));

                var model = _mapper.Map<CurrencyToggleUpdatingRatesModel>(dto);

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                throw;
            }
        }

        // POST: CurrenciesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CurrencyToggleUpdatingRatesModel model)
        {
            try
            {
                await _currenciesService.ToggleUpdateRatesAsync(model.Id, model.UpdateRates, model.AvailableForUsers);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Editing Currencies");
                ModelState.AddModelError("", "Something went wrong. Please, try again later or connect with admininstrator.");
                return View(model);
            }
        }

        // GET: CurrenciesController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            return NotFound();
        }

        // POST: CurrenciesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {

            return NotFound();

            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
