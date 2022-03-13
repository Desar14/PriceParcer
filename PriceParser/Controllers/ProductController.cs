﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PriceParser.Core.Interfaces;
using PriceParser.Models;

namespace PriceParser.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductsService _productService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductsService productService, IMapper mapper, ILogger<ProductController> logger)
        {
            _productService = productService;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: ProductController
        public async Task<IActionResult> Index()
        {
            var products = (await _productService.GetAllProductsAsync())
                .Select(product => _mapper.Map<ProductItemListViewModel>(product))
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
        public async Task<IActionResult> Create(ProductCreateEditViewModel model)
        {
            model.Id = new Guid();
            var productToAdd = _mapper.Map<Core.DTO.ProductDTO>(model);
            try
            {
                await _productService.AddProduct(productToAdd);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: ProductController/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {

            var productDetailDTO = (await _productService.GetProductDetailsAsync(id));

            var model = _mapper.Map<ProductCreateEditViewModel>(productDetailDTO);

            return View(model);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductCreateEditViewModel model)
        {
            var productToAdd = _mapper.Map<Core.DTO.ProductDTO>(model);
            try
            {
                await _productService.EditProduct(productToAdd);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
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
            try
            {
                await _productService.DeleteProduct(model.Id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }
    }
}
