using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại hoặc email")]
        [Display(Name = "Số điện thoại hoặc email")]
        public string PhoneOrEmail { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; } = null!;

        public string CountryCode { get; set; } = "+84";
        public bool RememberMe { get; set; }
    }
}
