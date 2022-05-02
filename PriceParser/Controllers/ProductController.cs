using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PriceParser.Core.Interfaces;
using PriceParser.Data.Entities;
using PriceParser.Models;
using PriceParser.Models.ProductPrice;

namespace PriceParser.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductsService _productService;
        private readonly IProductPricesService _productPricesService;
        private readonly ICurrenciesService _currenciesService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductController(IProductsService productService, IMapper mapper, ILogger<ProductController> logger, IProductPricesService productPricesService, ICurrenciesService currenciesService, UserManager<ApplicationUser> userManager)
        {
            _productService = productService;
            _mapper = mapper;
            _logger = logger;
            _productPricesService = productPricesService;
            _currenciesService = currenciesService;
            _userManager = userManager;
        }
        [AllowAnonymous]
        // GET: ProductController
        public async Task<IActionResult> Index()
        {
            try
            {
                var products = (await _productService.GetAllProductsAsync())
                       .Select(product => _mapper.Map<ProductItemListViewModel>(product))
                       .OrderByDescending(product => product.Name).ToList();
                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                throw;
            }
        }
        [AllowAnonymous]
        // GET: ProductController/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);

                var productDetailDTO = (await _productService.GetProductDetailsAsync(id));

                var model = _mapper.Map<ProductDetailsViewModel>(productDetailDTO);

                foreach (var site in model.marketSites)
                {
                    var lastPrice = await _productPricesService.GetLastProductPriceAsync(site.Id) ?? new();
                    site.Price = lastPrice.FullPrice;
                    site.CurrencyCode = lastPrice.CurrencyCode;
                }

                model.Currencies = (await _currenciesService.GetUsableAsync())
                    .Select(curr => _mapper.Map<Core.DTO.CurrencyDTO, SelectListItem>(curr,
                        opt => opt.AfterMap((src, dest) =>
                        {
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


        // GET: ProductController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateEditViewModel model)
        {
            model.Id = new Guid();
            var productToAdd = _mapper.Map<Core.DTO.ProductDTO>(model);
            try
            {
                await _productService.AddProductAsync(productToAdd);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Creating Product");
                ModelState.AddModelError("", "Something went wrong. Please, try again later or connect with admininstrator.");
                return View(model);
            }
        }

        // GET: ProductController/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {

            try
            {
                var productDetailDTO = (await _productService.GetProductDetailsAsync(id));

                var model = _mapper.Map<ProductCreateEditViewModel>(productDetailDTO);

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                throw;
            }
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductCreateEditViewModel model)
        {
            var productToAdd = _mapper.Map<Core.DTO.ProductDTO>(model);
            try
            {
                await _productService.EditProductAsync(productToAdd);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Editing Product");
                ModelState.AddModelError("", "Something went wrong. Please, try again later or connect with admininstrator.");
                return View(model);
            }
        }

        // GET: ProductController/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {

            try
            {
                var productDetailDTO = (await _productService.GetProductDetailsAsync(id));

                var model = _mapper.Map<ProductDeleteViewModel>(productDetailDTO);

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                throw;
            }
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ProductDeleteViewModel model)
        {
            try
            {
                await _productService.DeleteProductAsync(model.Id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Deleting Product");
                ModelState.AddModelError("", "Something went wrong. Please, try again later or connect with admininstrator.");
                return View(model);
            }
        }
        [AllowAnonymous]
        public async Task<IActionResult> PricesData(Guid id, DateTime? startPeriod, DateTime? endPeriod, Guid? currencyId)
        {
            try
            {
                var prices = await _productPricesService.GetAllProductPricesPerSiteAsync(id, startPeriod, endPeriod, true);

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
