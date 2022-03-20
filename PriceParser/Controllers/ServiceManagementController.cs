﻿using Microsoft.AspNetCore.Mvc;
using PriceParser.Core.Interfaces;

namespace PriceParser.Controllers
{
    public class ServiceManagementController : Controller
    {
        private readonly IParsingPricesService _parsingPricesService;
        private readonly ILogger<ServiceManagementController> _logger;

        public ServiceManagementController(IParsingPricesService parsingPricesService, ILogger<ServiceManagementController> logger)
        {
            _parsingPricesService = parsingPricesService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ParsePrices()
        {
            try
            {
                await _parsingPricesService.ParseSaveAllAvailablePricesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Parsing prices");
            }            

            return RedirectToAction("Index");

        }

    }
}
