using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PriceParcer.Core.Interfaces;

namespace PriceParcer.Controllers
{
    public class MarketSitesController : Controller
    {
        private readonly IMarketSitesService _marketService;
        private readonly IMapper _mapper;

        public MarketSitesController(IMarketSitesService marketService, IMapper mapper)
        {
            _marketService = marketService;
            _mapper = mapper;
        }

        // GET: MarketSitesController
        public ActionResult Index()
        {
            return View();
        }

        // GET: MarketSitesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MarketSitesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MarketSitesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MarketSitesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MarketSitesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MarketSitesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MarketSitesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
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
