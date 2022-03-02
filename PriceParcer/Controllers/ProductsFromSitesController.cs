using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using PriceParcer.Core.Interfaces;
using PriceParcer.Models.ProductFromSite;

namespace PriceParcer.Controllers
{
    public class ProductsFromSitesController : Controller
    {

        private readonly IProductsFromSitesService _productsFromSitesService;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMarketSitesService _marketSiteService;
        private readonly IProductsService _productService;
        private readonly IProductPricesService _productPricesService;

        public ProductsFromSitesController(IProductsFromSitesService productsFromSitesService, IMapper mapper, UserManager<IdentityUser> userManager, IMarketSitesService marketService, IProductsService productService, IProductPricesService productPricesService)
        {
            _productsFromSitesService = productsFromSitesService;
            _mapper = mapper;
            _userManager = userManager;
            _marketSiteService = marketService;
            _productService = productService;
            _productPricesService = productPricesService;
        }

        // GET: ProductsFromSitesController
        public async Task<IActionResult> Index()
        {
            var products = (await _productsFromSitesService.GetAllAsync())
                .Select(product => _mapper.Map<ProductFromSiteItemListViewModel>(product))
                .OrderByDescending(product => product.Created).ToList();
            return View(products);
        }

        // GET: ProductsFromSitesController/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var productDetailDTO = (await _productsFromSitesService.GetDetailsAsync(id));

            var model = _mapper.Map<ProductFromSiteDetailsViewModel>(productDetailDTO);

            return View(model);
        }

        // GET: ProductsFromSitesController/Create
        public async Task<IActionResult> Create()
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
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: ProductsFromSitesController/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var recordDetailDTO = (await _productsFromSitesService.GetDetailsAsync(id));

            var model = _mapper.Map<ProductFromSiteCreateEditViewModel>(recordDetailDTO);

            model.UsersList = _userManager.Users.ToList()
                .Select(product => _mapper.Map<IdentityUser, SelectListItem>(product, opt => opt.AfterMap((src, dest) => dest.Selected = src.Id == model.CreatedByUserId))).ToList();
            model.ProductsList = (await _productService.GetAllProductsAsync())
                .Select(product => _mapper.Map<Core.DTO.ProductDTO, SelectListItem>(product, opt => opt.AfterMap((src, dest) => dest.Selected = src.Id == model.ProductId))).ToList();            
            model.SitesList = (await _marketSiteService.GetAllSitesAsync())
                .Select(site => _mapper.Map<Core.DTO.MarketSiteDTO,SelectListItem>(site, opt => opt.AfterMap((src, dest) => dest.Selected = src.Id == model.SiteId))).ToList();

            return View(model);
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
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: ProductsFromSitesController/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {

            var recordDetailDTO = (await _productsFromSitesService.GetDetailsAsync(id));

            var model = _mapper.Map<ProductFromSiteDeleteViewModel>(recordDetailDTO);

            return View(model);
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
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        public async Task<IActionResult> ParsePrice(Guid id)
        {
            try
            {        
                var dto = await _productPricesService.ParseProductPriceAsync(id);

                await _productPricesService.AddProductPriceAsync(dto);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }
    }
}
