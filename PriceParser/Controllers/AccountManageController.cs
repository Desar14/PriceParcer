using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PriceParser.Core.Interfaces;
using PriceParser.Data.Entities;
using PriceParser.Models.Account.Manage;

namespace PriceParser.Controllers
{
    public class AccountManageController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICurrenciesService _currService;
        private readonly IMapper _mapper;

        public AccountManageController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ICurrenciesService currService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _currService = currService;
            _mapper = mapper;
        }

        private async Task LoadAsync(ApplicationUser user, IndexModel model)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            var currSelectList = (await _currService.GetUsableAsync())
                    .Select(curr => _mapper.Map<Core.DTO.CurrencyDTO, SelectListItem>(curr, opt => opt.AfterMap((src, dest) => dest.Selected = src.Id == user.UserCurrencyId))).ToList();
            model.CurrencySelectList = currSelectList;
            model.Username = userName;
            model.IsPhoneConfirmed = await _userManager.IsPhoneNumberConfirmedAsync(user);
            model.Input = new IndexModel.InputModel
            {
                PhoneNumber = phoneNumber,
                UserCurrencyId = user.UserCurrencyId
            };
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var model = new IndexModel();
            await LoadAsync(user, model);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Index(IndexModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user,model);
                return View(model);
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (model.Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, model.Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    model.StatusMessage = "Unexpected error when trying to set phone number.";
                    return View(model);
                }
            }

            if (model.Input.UserCurrencyId != user.UserCurrencyId)
            {
                user.UserCurrencyId = model.Input.UserCurrencyId;
                await _userManager.UpdateAsync(user);
            }

            await _signInManager.RefreshSignInAsync(user);
            model.StatusMessage = "Your profile has been updated";
            LoadAsync(user, model);
            return View(model);
        }

        public async Task<IActionResult> ValidatePhone()
        {
            return RedirectToPage("/Identity/Account/VerifyPhone");
        }
    }
}
