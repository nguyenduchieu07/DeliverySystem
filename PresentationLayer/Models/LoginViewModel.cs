using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models
{
    public class LoginViewModel
    {

        [Required]
        public string PhoneOrEmail { get; set; } = null!;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public string CountryCode { get; set; } = "+84";
        public bool RememberMe { get; set; }
    }
}
