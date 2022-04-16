using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using PriceParser.Core.Interfaces;
using PriceParser.Data.Entities;
using PriceParser.Models.ProductFromSite;
using PriceParser.Models.ProductPrice;

namespace PriceParser.Controllers
{
    public class ProductsFromSitesController : Controller
    {

        private readonly IProductsFromSitesService _productsFromSitesService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMarketSitesService _marketSiteService;
        private readonly IProductsService _productService;
        private readonly IProductPricesService _productPricesService;
        private readonly IParsingPricesService _parsingPricesService;
        private readonly ICurrenciesService _currenciesService;
        private readonly ILogger<ProductsFromSitesController> _logger;

        public ProductsFromSitesController(IProductsFromSitesService productsFromSitesService, IMapper mapper, UserManager<ApplicationUser> userManager, IMarketSitesService marketService, IProductsService productService, IProductPricesService productPricesService, IParsingPricesService parsingPricesService, ILogger<ProductsFromSitesController> logger, ICurrenciesService currenciesService)
        {
            _productsFromSitesService = productsFromSitesService;
            _mapper = mapper;
            _userManager = userManager;
            _marketSiteService = marketService;
            _productService = productService;
            _productPricesService = productPricesService;
            _parsingPricesService = parsingPricesService;
            _logger = logger;
            _currenciesService = currenciesService;
        }

        // GET: ProductsFromSitesController
        public async Task<IActionResult> Index()
        {
            try
            {
                var products = (await _productsFromSitesService.GetAllAsync())
                       .Select(product => _mapper.Map<ProductFromSiteItemListViewModel>(product))
                       .OrderByDescending(product => product.Created).ToList();
                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                throw;
            }
        }

        // GET: ProductsFromSitesController/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var productDetailDTO = (await _productsFromSitesService.GetDetailsAsync(id));
                var prices = (await _productPricesService.GetAllProductPricesAsync(id)).Select(priceItem => _mapper.Map<ProductPriceItemListViewModel>(priceItem)).OrderByDescending(price => price.ParseDate).ToList();
                var currentUser = await _userManager.GetUserAsync(User);

                var model = _mapper.Map<ProductFromSiteDetailsViewModel>(productDetailDTO);
                model.productPrices = prices;

                model.Currencies = (await _currenciesService.GetUsableAsync())
                    .Select(curr => _mapper.Map<Core.DTO.CurrencyDTO, SelectListItem>(curr,
                        opt => opt.AfterMap((src, dest) => {
                            if (currentUser?.UserCurrencyId == null)
                            {
                                dest.Selected = src.Cur_Abbreviation == "BYN";
                            }
                            else
                            {
                                dest.Selected = currentUser.UserCurrencyId == src.Id;
                            }
                        }
                     ))).ToList();

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                throw;
            }
        }

        // GET: ProductsFromSitesController/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                var model = new ProductFromSiteCreateEditViewModel
                {
                    Id = Guid.NewGuid(),
                    Created = DateTime.Now,
                    DoNotParse = false,
                    UsersList = _userManager.Users.Select(product => _mapper.Map<SelectListItem>(product)).ToList(),
                    ProductsList = (await _productService.GetAllProductsAsync()).Select(product => _mapper.Map<SelectListItem>(product)).ToList(),
                    SitesList = (await _marketSiteService.GetAllSitesAsync()).Select(site => _mapper.Map<SelectListItem>(site)).ToList()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                throw;
            }
        }

        // POST: ProductsFromSitesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductFromSiteCreateEditViewModel model)
        {
            var recordToAdd = _mapper.Map<Core.DTO.ProductFromSitesDTO>(model);

            try
            {
                await _productsFromSitesService.AddAsync(recordToAdd);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Creating Product from site");
                ModelState.AddModelError("", "Something went wrong. Please, try again later or connect with admininstrator.");
                return View(model);
            }
        }

        // GET: ProductsFromSitesController/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var recordDetailDTO = (await _productsFromSitesService.GetDetailsAsync(id));

                var model = _mapper.Map<ProductFromSiteCreateEditViewModel>(recordDetailDTO);

                model.UsersList = _userManager.Users.ToList()
                    .Select(product => _mapper.Map<ApplicationUser, SelectListItem>(product, opt => opt.AfterMap((src, dest) => dest.Selected = src.Id == model.CreatedByUserId))).ToList();
                model.ProductsList = (await _productService.GetAllProductsAsync())
                    .Select(product => _mapper.Map<Core.DTO.ProductDTO, SelectListItem>(product, opt => opt.AfterMap((src, dest) => dest.Selected = src.Id == model.ProductId))).ToList();
                model.SitesList = (await _marketSiteService.GetAllSitesAsync())
                    .Select(site => _mapper.Map<Core.DTO.MarketSiteDTO, SelectListItem>(site, opt => opt.AfterMap((src, dest) => dest.Selected = src.Id == model.SiteId))).ToList();

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                throw;
            }
        }

        // POST: ProductsFromSitesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductFromSiteCreateEditViewModel model)
        {

            var recordToAdd = _mapper.Map<Core.DTO.ProductFromSitesDTO>(model);

            try
            {
                await _productsFromSitesService.EditAsync(recordToAdd);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Editing Product from site");
                ModelState.AddModelError("", "Something went wrong. Please, try again later or connect with admininstrator.");
                return View(model);
            }
        }

        // GET: ProductsFromSitesController/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {

            try
            {
                var recordDetailDTO = (await _productsFromSitesService.GetDetailsAsync(id));

                var model = _mapper.Map<ProductFromSiteDeleteViewModel>(recordDetailDTO);

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                throw;
            }
        }

        // POST: ProductsFromSitesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ProductFromSiteDeleteViewModel model)
        {
            try
            {
                await _productsFromSitesService.DeleteAsync(model.Id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Deleting Product from site");
                ModelState.AddModelError("", "Something went wrong. Please, try again later or connect with admininstrator.");
                return View(model);
            }
        }

        public async Task<IActionResult> ParsePrice(Guid id)
        {
            try
            {
                var dto = await _parsingPricesService.ParseSaveProductPriceAsync(id);

                return RedirectToAction(nameof(Details), new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Parsing Product from site {id}");
                ModelState.AddModelError("", "Something went wrong. Please, try again later or connect with admininstrator.");
                return RedirectToAction(nameof(Details), new { id });
            }
        }

        public async Task<IActionResult> PricesData(Guid id, DateTime? startPeriod, DateTime? endPeriod, Guid? currencyId)
        {
            try
            {
                var prices = await _productPricesService.GetAllProductFromSitePricesAsync(id, startPeriod, endPeriod, true);
                
                var currency = await _currenciesService.GetDetailsAsync(currencyId ?? Guid.Empty);

                if (currencyId != null && currency != null)
                {
                    foreach (var item in prices)
                    {
                        item.Prices = (await _currenciesService.ConvertAtTheRate(item.Prices, currencyId.Value)).ToList();
                    }
                }

                var result = prices.Select(x => _mapper.Map<ProductPricesPerSiteDataItemModel>(x, opt => opt.AfterMap((src, dest) => dest.CurrencyCode = currency?.Cur_Abbreviation)));

                return Json(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
