using Hangfire;
using Hangfire.Storage;
using Microsoft.AspNetCore.Mvc;
using PriceParser.Core.Interfaces;
using PriceParser.Models.ServiceManagement;

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
            var model = new ServiceManagementIndexModel();

            IStorageConnection connection = JobStorage.Current.GetConnection();
            
            var jobsById = connection.GetRecurringJobs(new List<string>() { "ParsingPricesFromSites" });

            if (jobsById.Count > 0 && jobsById[0].Job != null)
            {
                model.ParsingPricesState = "enabled";
            }
            else
            {
                model.ParsingPricesState = "disabled";
            }
            
            return View(model);
        }

        public async Task<IActionResult> ParsePrices()
        {
            try
            {
                BackgroundJob.Enqueue(() =>  _parsingPricesService.ParseSaveAllAvailablePricesAsync());                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Parsing prices");
            }            

            return RedirectToAction("Index");

        }

        public async Task<IActionResult> ToggleParsingPricesBackground(string? newState)
        {

            if (newState == "enable")
            {
                try
                {
                    RecurringJob.AddOrUpdate(
                        "ParsingPricesFromSites",
                        () => _parsingPricesService.ParseSaveAllAvailablePricesAsync(),
                        Cron.Hourly
                        );
                }
                catch (Exception ex)
                {

                    _logger.LogError(ex, "An error occurred while adding a recurring job");
                }
            }else if (newState == "disable")
            {
                try
                {
                    RecurringJob.RemoveIfExists(
                        "ParsingPricesFromSites"
                        );
                }
                catch (Exception ex)
                {

                    _logger.LogError(ex, "An error occurred while removing a recurring job");
                }
            }
            
            

            return RedirectToAction("Index");
        }

    }
}
