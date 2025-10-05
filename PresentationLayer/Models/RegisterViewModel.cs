using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Vui lòng chọn mã vùng")]
        public string CountryCode { get; set; } = "+84";

        [Required(ErrorMessage = "Số điện thoại bắt buộc")]
        [RegularExpression(@"^[0-9]{8,11}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Họ và tên bắt buộc")]
        [StringLength(100, ErrorMessage = "Họ tên tối đa 100 ký tự")]
        public string FullName { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu bắt buộc")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu ít nhất 6 ký tự")]
        public string Password { get; set; }

        // Sửa: Thêm Required và Compare để kiểm tra xác nhận mật khẩu
        [Required(ErrorMessage = "Xác nhận mật khẩu bắt buộc")]
        [Compare("Password", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp")]
        public string ConfirmPassword { get; set; } = null!;

        [Required(ErrorMessage = "Bạn cần đồng ý với điều khoản")]
        public bool AgreeToTerms { get; set; }
    }
}