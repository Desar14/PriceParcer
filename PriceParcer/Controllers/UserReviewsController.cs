using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PriceParcer.Core.Interfaces;
using PriceParcer.Models.UserReview;

namespace PriceParcer.Controllers
{
    public class UserReviewsController : Controller
    {

        private readonly IUserReviewsService _reviewsService;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IProductsService _productService;

        public UserReviewsController(IUserReviewsService reviewsService, IMapper mapper, UserManager<IdentityUser> userManager, IProductsService productService)
        {
            _reviewsService = reviewsService;
            _mapper = mapper;
            _userManager = userManager;
            _productService = productService;
        }

        // GET: UserReviewsController
        public async Task<IActionResult> Index()
        {
            var reviews = (await _reviewsService.GetAllAsync())
                .Select(product => _mapper.Map<UserReviewItemListViewModel>(product))
                .OrderByDescending(product => product.ReviewDate).ToList();
            return View(reviews);
        }

        // GET: UserReviewsController/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var reviewDetailDTO = (await _reviewsService.GetDetailsAsync(id));

            var model = _mapper.Map<UserReviewDetailsViewModel>(reviewDetailDTO);

            return View(model);
        }

        // GET: UserReviewsController/Create
        public async Task<IActionResult> Create()
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
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: UserReviewsController/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var recordDetailDTO = (await _reviewsService.GetDetailsAsync(id));

            var model = _mapper.Map<UserReviewCreateEditViewModel>(recordDetailDTO);

            model.UsersList = _userManager.Users.ToList()
                .Select(product => _mapper.Map<IdentityUser, SelectListItem>(product, opt => opt.AfterMap((src, dest) => dest.Selected = src.Id == model.UserId))).ToList();
            model.ProductsList = (await _productService.GetAllProductsAsync())
                .Select(product => _mapper.Map<Core.DTO.ProductDTO, SelectListItem>(product, opt => opt.AfterMap((src, dest) => dest.Selected = src.Id == model.ProductId))).ToList();            

            return View(model);
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
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: UserReviewsController/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var recordDetailDTO = (await _reviewsService.GetDetailsAsync(id));

            var model = _mapper.Map<UserReviewDeleteViewModel>(recordDetailDTO);

            return View(model);
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
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }
    }
}
