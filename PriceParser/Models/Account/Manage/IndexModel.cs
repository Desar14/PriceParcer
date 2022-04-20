using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PriceParser.Models.Account.Manage
{
    public class IndexModel
    {
        
        public string Username { get; set; }        
        [TempData, ValidateNever]
        public string StatusMessage { get; set; }    
        [BindProperty]
        public InputModel Input { get; set; }
        public List<SelectListItem> CurrencySelectList { get; set; }

        public bool IsPhoneConfirmed { get; set; }
        
        public class InputModel
        {
            
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            [Display(Name = "Default currency")]
            public Guid? UserCurrencyId { get; set; }


        }
    }
}
