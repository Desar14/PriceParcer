using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PriceParser.Models.Account
{
    public class LoginModel
    {
        [BindProperty]
        public InputModel Input { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public string ReturnUrl { get; set; }
        
        public class InputModel
        {

            [Required]
            [EmailAddress]
            public string Email { get; set; }


            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }


            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
    }
    //public class LoginModel
    //{

    //    [Required]
    //    [EmailAddress]
    //    public string Email { get; set; }


    //    [Required]
    //    [DataType(DataType.Password)]
    //    public string Password { get; set; }


    //    [Display(Name = "Remember me?")]
    //    public bool RememberMe { get; set; }

    //    public IList<AuthenticationScheme> ExternalLogins { get; set; }

    //    public string ReturnUrl { get; set; }

    //    [TempData]
    //    public string ErrorMessage { get; set; }
    //}

}
