﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PriceParser.Core.Interfaces;
using PriceParser.Data.Entities;
using PriceParser.Models;
using PriceParser.Models.UserReview;

namespace PriceParser.Controllers
{
    [Authorize]
    public class UserReviewsController : Controller
    {

        private readonly IUserReviewsService _reviewsService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProductsService _productService;
        private readonly ILogger<UserReviewsController> _logger;

        public UserReviewsController(IUserReviewsService reviewsService, IMapper mapper, UserManager<ApplicationUser> userManager, IProductsService productService, ILogger<UserReviewsController> logger)
        {
            _reviewsService = reviewsService;
            _mapper = mapper;
            _userManager = userManager;
            _productService = productService;
            _logger = logger;
        }

        // GET: UserReviewsController
        public async Task<IActionResult> Index()
        {
            try
            {
                var reviews = (await _reviewsService.GetAllAsync())
                        .Select(product => _mapper.Map<UserReviewItemListViewModel>(product))
                        .OrderByDescending(product => product.ReviewDate).ToList();
                return View(reviews);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                throw;
            }
        }

        // GET: UserReviewsController/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var reviewDetailDTO = (await _reviewsService.GetDetailsAsync(id));

                var model = _mapper.Map<UserReviewDetailsViewModel>(reviewDetailDTO);

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                throw;
            }
        }

        // GET: UserReviewsController/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                var model = new UserReviewCreateEditViewModel
                {
                    Id = Guid.NewGuid(),
                    ReviewDate = DateTime.Now,
                    Hidden = false,
                    UsersList = _userManager.Users.Select(product => _mapper.Map<SelectListItem>(product)).ToList(),
                    ProductsList = (await _productService.GetAllProductsAsync()).Select(product => _mapper.Map<SelectListItem>(product)).ToList()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                throw;
            }
        }

        // POST: UserReviewsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserReviewCreateEditViewModel model)
        {
            var recordToAdd = _mapper.Map<Core.DTO.UserReviewDTO>(model);

            try
            {
                await _reviewsService.AddAsync(recordToAdd);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Creating User review");
                ModelState.AddModelError("", "Something went wrong. Please, try again later or connect with admininstrator.");
                return View(model);
            }
        }

        // GET: UserReviewsController/Create
        public async Task<IActionResult> CreateFromProduct(Guid productId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                var model = new UserReviewCreateEditViewModel
                {
                    Id = Guid.NewGuid(),
                    ReviewDate = DateTime.Now,
                    Hidden = false,
                    UserId = user.Id,
                    User = user,
                    UsersList = _userManager.Users.Select(product => _mapper.Map<SelectListItem>(product)).ToList(),
                    ProductId = productId,
                    Product = _mapper.Map<ProductItemListViewModel>(await _productService.GetProductDetailsAsync(productId))
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                throw;
            }
        }

        // POST: UserReviewsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFromProduct(UserReviewCreateEditViewModel model)
        {
            var recordToAdd = _mapper.Map<Core.DTO.UserReviewDTO>(model);

            try
            {
                await _reviewsService.AddAsync(recordToAdd);
                return RedirectToAction("Details", "Product", new { id = recordToAdd.ProductId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Creating User review from product");
                ModelState.AddModelError("", "Something went wrong. Please, try again later or connect with admininstrator.");
                return View(model);
            }
        }

        // GET: UserReviewsController/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var recordDetailDTO = (await _reviewsService.GetDetailsAsync(id));

                var model = _mapper.Map<UserReviewCreateEditViewModel>(recordDetailDTO);

                model.UsersList = _userManager.Users.ToList()
                    .Select(product => _mapper.Map<ApplicationUser, SelectListItem>(product, opt => opt.AfterMap((src, dest) => dest.Selected = src.Id == model.UserId))).ToList();
                model.ProductsList = (await _productService.GetAllProductsAsync())
                    .Select(product => _mapper.Map<Core.DTO.ProductDTO, SelectListItem>(product, opt => opt.AfterMap((src, dest) => dest.Selected = src.Id == model.ProductId))).ToList();

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                throw;
            }
        }

        // POST: UserReviewsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserReviewCreateEditViewModel model)
        {
            var recordToAdd = _mapper.Map<Core.DTO.UserReviewDTO>(model);

            try
            {
                await _reviewsService.EditAsync(recordToAdd);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Editing User review");
                ModelState.AddModelError("", "Something went wrong. Please, try again later or connect with admininstrator.");
                return View(model);
            }
        }

        // GET: UserReviewsController/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var recordDetailDTO = (await _reviewsService.GetDetailsAsync(id));

                var model = _mapper.Map<UserReviewDeleteViewModel>(recordDetailDTO);

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                throw;
            }
        }

        // POST: UserReviewsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(UserReviewDeleteViewModel model)
        {
            try
            {
                await _reviewsService.DeleteAsync(model.Id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Deleting User review");
                ModelState.AddModelError("", "Something went wrong. Please, try again later or connect with admininstrator.");
                return View(model);
            }
        }
    }
}
