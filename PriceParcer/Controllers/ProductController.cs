using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PriceParcer.Core.Interfaces;
using PriceParcer.Models;

namespace PriceParcer.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductsService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductsService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        // GET: ProductController
        public async Task<IActionResult> Index()
        {
            var products = (await _productService.GetAllProductsAsync())
                .Select(product => _mapper.Map<ProductItemListModel>(product))
                .OrderByDescending(product => product.Name).ToList();
            return View(products);
        }

        // GET: ProductController/Details/5
        public async Task<IActionResult> Details(Guid id)
        {   
            var productDetailDTO = (await _productService.GetProductDetailsAsync(id));
            
            var model = _mapper.Map<ProductDetailsViewModel>(productDetailDTO);

            return View(model);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEditProductViewModel model)
        {
            model.Id = new Guid();
            var productToAdd = _mapper.Map<Core.DTO.ProductDTO>(model);
            try
            {
                await _productService.AddProduct(productToAdd);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }

        // GET: ProductController/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {

            var productDetailDTO = (await _productService.GetProductDetailsAsync(id));

            var model = _mapper.Map<CreateEditProductViewModel>(productDetailDTO);

            return View(model);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateEditProductViewModel model)
        {
            var productToAdd = _mapper.Map<Core.DTO.ProductDTO>(model);
            //try
            //{
                await _productService.EditProduct(productToAdd);
                return RedirectToAction(nameof(Index));
            //}
            //catch
            //{
            //    return View(model);
            //}
        }

        // GET: ProductController/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {

            var productDetailDTO = (await _productService.GetProductDetailsAsync(id));

            var model = _mapper.Map<ProductDeleteViewModel>(productDetailDTO);

            return View(model);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ProductDeleteViewModel model)
        {
            //try
            //{
                await _productService.DeleteProduct(model.Id);
                return RedirectToAction(nameof(Index));
            //}
            //catch
            //{
            //    return View(model);
            //}
        }
    }
}
