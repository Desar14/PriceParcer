using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceParser.Api.Models.Product;
using PriceParser.Core.DTO;
using PriceParser.Core.Interfaces;
using PriceParser.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PriceParser.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductsService _productsService;
        private readonly IProductPricesService _productPricesService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductsService productsService, IMapper mapper, ILogger<ProductController> logger, IProductPricesService productPricesService)
        {
            _productsService = productsService;
            _mapper = mapper;
            _logger = logger;
            _productPricesService = productPricesService;
        }

        // GET: api/<ProductController>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int page)
        {
            var products = (await _productsService.GetAllProductsAsync(page)).Select(x => _mapper.Map<GetProductModel>(x));


            foreach (var product in products)
                foreach (var site in product.FromSites)
                {
                    var lastPrice = await _productPricesService.GetLastProductPriceAsync(site.Id) ?? new();
                    site.Price = lastPrice.FullPrice;
                    site.CurrencyCode = lastPrice.CurrencyCode;
                }

            return Ok(products);
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid id)
        {
            var productDTO = await _productsService.GetProductDetailsAsync(id);

            var product = _mapper.Map<GetProductModel>(productDTO);

            foreach (var site in product.FromSites)
            {
                var lastPrice = await _productPricesService.GetLastProductPriceAsync(site.Id) ?? new();
                site.Price = lastPrice.FullPrice;
                site.CurrencyCode = lastPrice.CurrencyCode;
            }

            return Ok(product);
        }

        // POST api/<ProductController>
        [HttpPost]        
        public async Task<IActionResult> Post([FromBody] PostPutProductModel value)
        {
            var productDTO = _mapper.Map<ProductDTO>(value);

            await _productsService.AddProductAsync(productDTO);

            return Ok();
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Put(Guid id, [FromBody] PostPutProductModel value)
        {
            var productDTO = _mapper.Map<ProductDTO>(value);

            var currentEntity = await _productsService.GetProductDetailsAsync(id);
            if (currentEntity == null)
            {
                return BadRequest($"Product with id {id} not found");
            }

            currentEntity.Name = value.Name;
            currentEntity.Category = value.Category;
            currentEntity.Description = value.Description;
            currentEntity.Hidden = value.Hidden;
            currentEntity.CurrencyCode = value.CurrencyCode;

            var result = await _productsService.EditProductAsync(productDTO);
            if (result)
            {
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }
            
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var currentEntity = await _productsService.GetProductDetailsAsync(id);
            if (currentEntity == null)
            {
                return BadRequest($"Product with id {id} not found");
            }
            var result = await _productsService.DeleteProductAsync(id);
            if (result)
            {
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }

        }
    }
}
