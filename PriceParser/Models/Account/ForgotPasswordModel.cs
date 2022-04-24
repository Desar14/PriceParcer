using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PriceParser.Models.Account
{
    public class ForgotPasswordModel
    {
        [BindProperty]
        public InputModel Input { get; set; }        
        public class InputModel        {
            
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }
    }
}
