using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PriceParser.Models.Account
{
    public class ConfirmPhoneModel
    {
        public string PhoneNumber { get; set; }

        [BindProperty, Required, Display(Name = "Code")]
        public string VerificationCode { get; set; }
    }
}
