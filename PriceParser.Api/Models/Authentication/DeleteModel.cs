using System.ComponentModel.DataAnnotations;

namespace PriceParser.Api.Models.Authentication
{
    public class DeleteModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }
    }
}
