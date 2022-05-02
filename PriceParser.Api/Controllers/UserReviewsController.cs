using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PriceParser.Api.Models.UserReview;
using PriceParser.Core.DTO;
using PriceParser.Core.Interfaces;
using PriceParser.Data;
using PriceParser.Data.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PriceParser.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserReviewsController : ControllerBase
    {
        private readonly IUserReviewsService _reviewsService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<UserReviewsController> _logger;

        public UserReviewsController(IUserReviewsService reviewsService,
                                     IMapper mapper,
                                     ILogger<UserReviewsController> logger,
                                     UserManager<ApplicationUser> userManager)
        {
            _reviewsService = reviewsService;
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
        }

        // GET: api/<UserReviewsController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = (await _reviewsService.GetAllAsync()).Select(x => _mapper.Map<GetUserReviewModel>(x));

            return Ok(result);
        }

        // GET api/<UserReviewsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var dto = await _reviewsService.GetDetailsAsync(id);

            var result = _mapper.Map<GetUserReviewModel>(dto);

            return Ok(result);
        }

        // POST api/<UserReviewsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostUserReviewModel value)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                if (user == null)
                {
                    return BadRequest($"User not found");
                }
                var dto = _mapper.Map<UserReviewDTO>(value);
                dto.UserId = user.Id;
                var result = await _reviewsService.AddAsync(dto);

                if (result)
                {
                    return Ok();
                }
                else
                    return StatusCode(500);
            }

            return BadRequest(ModelState.ValidationState);
        }

        // PUT api/<UserReviewsController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Moderator)]
        public async Task<IActionResult> Put(Guid id, [FromBody] PutUserReviewModel value)
        {
            var currentEntity = await _reviewsService.GetDetailsAsync(id);

            if (currentEntity == null)
                return BadRequest($"User review with id {id} not found");

            if (ModelState.IsValid)
            {
                var dto = _mapper.Map<UserReviewDTO>(value);

                currentEntity.UserId = dto.UserId;
                currentEntity.ProductId = dto.ProductId;
                currentEntity.ReviewTitle = dto.ReviewTitle;
                currentEntity.ReviewText = dto.ReviewText;
                currentEntity.ReviewScore = dto.ReviewScore;
                currentEntity.ReviewDate = dto.ReviewDate;
                currentEntity.Hidden = dto.Hidden;

                var result = await _reviewsService.EditAsync(currentEntity);

                if (result)
                {
                    return Ok();
                }
                else
                    return StatusCode(500);
            }

            return BadRequest(ModelState.ValidationState);
        }

        // DELETE api/<UserReviewsController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Moderator)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var currentEntity = await _reviewsService.GetDetailsAsync(id);
            if (currentEntity == null)
            {
                return BadRequest($"User review with id {id} not found");
            }
            var result = await _reviewsService.DeleteAsync(id);
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
