using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PriceParser.Api.Models.Currency;
using PriceParser.Core.Interfaces;
using PriceParser.Data;
using PriceParser.Data.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PriceParser.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Moderator)]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrenciesService _currencyService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<CurrencyController> _logger;

        public CurrencyController(ICurrenciesService currencyService, UserManager<ApplicationUser> userManager, IMapper mapper, ILogger<CurrencyController> logger)
        {
            _currencyService = currencyService;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/<CurrencyController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = (await _currencyService.GetAllAsync()).Select(x => _mapper.Map<GetCurrencyModel>(x));
            return Ok(result);
        }

        [HttpGet("usable")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUsable()
        {
            var result = (await _currencyService.GetUsableAsync()).Select(x => _mapper.Map<GetCurrencyModel>(x));
            return Ok(result);
        }

        [HttpGet("userdefault")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Moderator + "," + UserRoles.User)]
        public async Task<IActionResult> GetUserDefault()
        {
            var user = await _userManager.FindByNameAsync(User.Identity?.Name);
            if (user == null)
                return BadRequest($"User not found");

            var dto = await _currencyService.GetDetailsAsync(user.UserCurrencyId.Value);
            var result = _mapper.Map<GetCurrencyModel>(dto);
            return Ok(result);
        }

        // GET api/<CurrencyController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var dto = await _currencyService.GetDetailsAsync(id);

            if (dto == null)
            {
                return BadRequest($"Currency with id {id} not found");
            }

            var result = _mapper.Map<GetCurrencyModel>(dto);
            return Ok(result);
        }

        // POST api/<CurrencyController>
        [HttpPost("fromNBRB")]
        public async Task<IActionResult> Post()
        {
            await _currencyService.AddFromNBRBAsync();

            return Ok();
        }

        [HttpPost("{id}/updateRates")]
        public async Task<IActionResult> Post(Guid id)
        {
            var entity = _currencyService.GetDetailsAsync(id);
            if (entity == null)
            {
                return BadRequest($"Currency with id {id} not found");
            }

            var result = await _currencyService.UpdateRatesAsync(id);

            if (result)
            {
                return Ok();
            }
            return StatusCode(500);
        }

        // PUT api/<CurrencyController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] PutCurrencyModel value)
        {
            var entity = await _currencyService.GetDetailsAsync(id);
            if (entity == null)
            {
                return BadRequest($"Currency with id {id} not found");
            }

            var result = await _currencyService.ToggleUpdateRatesAsync(id, value.UpdateRates, value.AvailableForUsers);

            if (result)
            {
                return Ok();
            }
            return StatusCode(500);
        }

        // DELETE api/<CurrencyController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var currentEntity = await _currencyService.GetDetailsAsync(id);
            if (currentEntity == null)
            {
                return BadRequest($"Currency with id {id} not found");
            }
            var result = await _currencyService.DeleteAsync(id);
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
