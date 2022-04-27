using System.ComponentModel.DataAnnotations;

namespace PriceParser.Api.Models.Authentication
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Old password is required")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "New password is required")]
        public string NewPassword { get; set; }
    }
}
