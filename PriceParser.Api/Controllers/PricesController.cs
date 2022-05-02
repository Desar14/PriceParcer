using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceParser.Api.Models.Prices;
using PriceParser.Core.DTO;
using PriceParser.Core.Interfaces;
using PriceParser.Data;

namespace PriceParser.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = UserRoles.Admin)]
    public class PricesController : ControllerBase
    {

        private readonly IProductPricesService _productPricesService;
        private readonly ICurrenciesService _currenciesService;
        private readonly IMapper _mapper;
        private readonly ILogger<PricesController> _logger;

        public PricesController(ILogger<PricesController> logger,
                                IProductPricesService productPricesService,
                                ICurrenciesService currenciesService,
                                IMapper mapper)
        {
            _logger = logger;
            _productPricesService = productPricesService;
            _currenciesService = currenciesService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<GetPricesResponseModel>),200)]
        public async Task<IActionResult> Get(Guid? prodId, Guid? prodFromSiteId, DateTime? startPeriod, DateTime? endPeriod, Guid? currencyId)
        {
            try
            {
                IEnumerable<ProductFromSitesDTO>? prices;

                if (prodFromSiteId != null)
                {
                    prices = await _productPricesService.GetAllProductFromSitePricesAsync(prodFromSiteId.Value, startPeriod, endPeriod, true);
                }
                else
                {
                    prices = await _productPricesService.GetAllProductPricesPerSiteAsync(prodId.Value, startPeriod, endPeriod, true);
                }

                var currency = await _currenciesService.GetDetailsAsync(currencyId ?? Guid.Empty);

                if (currencyId != null && currency != null)
                {
                    foreach (var item in prices)
                    {
                        item.Prices = (await _currenciesService.ConvertAtTheRate(item.Prices, currencyId.Value)).ToList();
                    }
                }

                var result = prices.Select(x => _mapper.Map<GetPricesResponseModel>(x, opt => opt.AfterMap((src, dest) => dest.CurrencyCode = currency?.Cur_Abbreviation)));

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<ProductPriceDTO>), 200)]
        public async Task<IActionResult> Get(Guid id)
        {
            var prices = await _productPricesService.GetAllProductPricesAsync(id);

            return Ok(prices);
        }

        [HttpPost]
        [Route("range")]
        public async Task<IActionResult> Post([FromBody]IEnumerable<PutProductPriceModel> prices)
        {
            var pricesToAdd = prices.Select(x => _mapper.Map<ProductPriceDTO>(x));
            var result = await _productPricesService.AddProductPricesRangeAsync(pricesToAdd);

            if (result)            
                return Ok("Success");            
            else
                return BadRequest("Fail");
        }

        [HttpPost]        
        public async Task<IActionResult> Post([FromBody]PutProductPriceModel price)
        {
            var pricesToAdd =  _mapper.Map<ProductPriceDTO>(price);
            var result = await _productPricesService.AddProductPriceAsync(pricesToAdd);

            if (result)
                return Ok("Success");
            else
                return BadRequest("Fail");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid Id)
        {
            
            var result = await _productPricesService.DeleteProductPriceAsync(Id);

            if (result)
                return Ok("Success");
            else
                return BadRequest("Fail");
        }
    }
}