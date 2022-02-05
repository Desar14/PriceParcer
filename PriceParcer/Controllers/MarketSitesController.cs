using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PriceParcer.Core.Interfaces;
using PriceParcer.Models;

namespace PriceParcer.Controllers
{
    public class MarketSitesController : Controller
    {
        private readonly IMarketSitesService _marketService;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public MarketSitesController(IMarketSitesService marketService, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _marketService = marketService;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET: MarketSitesController
        public async Task<IActionResult> Index()
        {
            var sites = await _marketService.GetAllSitesAsync();

            var model = _mapper.Map<IEnumerable<MarketSiteListItemViewModel>>(sites);
            
            return View(model);
        }

        // GET: MarketSitesController/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var siteDetailDTO = await _marketService.GetSiteDetailsAsync(id);

            var model = _mapper.Map<MarketSiteDetailsViewModel>(siteDetailDTO);
            return View(model);
        }

        // GET: MarketSitesController/Create
        public async Task<IActionResult> Create()
        {
            var model = new MarketSiteCreateEditViewModel
            {
                Id = Guid.NewGuid(),
                Created = DateTime.Now,
                IsAvailable = true,
                UsersList = _userManager.Users.Select(product => _mapper.Map<SelectListItem>(product)).ToList()
            };

            return View(model);
        }

        // POST: MarketSitesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MarketSiteCreateEditViewModel model)
        {

            var siteToAdd = _mapper.Map<Core.DTO.MarketSiteDTO>(model);

            try
            {
                await _marketService.AddSite(siteToAdd);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: MarketSitesController/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {

            var siteDetailDTO = (await _marketService.GetSiteDetailsAsync(id));

            var model = _mapper.Map<MarketSiteCreateEditViewModel>(siteDetailDTO);

            model.UsersList = _userManager.Users.Select(product => _mapper.Map<SelectListItem>(product)).ToList();

            return View(model);

        }

        // POST: MarketSitesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MarketSiteCreateEditViewModel model)
        {

            var siteToAdd = _mapper.Map<Core.DTO.MarketSiteDTO>(model);

            try
            {
                await _marketService.EditSite(siteToAdd);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: MarketSitesController/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {

            var siteDetailDTO = (await _marketService.GetSiteDetailsAsync(id));

            var model = _mapper.Map<MarketSiteDeleteViewModel>(siteDetailDTO);

            return View(model);
        }

        // POST: MarketSitesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(MarketSiteDeleteViewModel model)
        {
            try
            {
                await _marketService.DeleteSite(model.Id);
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
