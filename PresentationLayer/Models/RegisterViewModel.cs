using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Vui lòng chọn mã vùng")]
        public string CountryCode { get; set; } = "+84";

        [Required(ErrorMessage = "Số điện thoại bắt buộc")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Số điện thoại phải có đúng 10 số")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Số điện thoại phải có đúng 10 số")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Họ và tên bắt buộc")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Họ tên phải có từ 2 đến 100 ký tự")]
        [RegularExpression(@"^[\p{L}\s'-]+$", ErrorMessage = "Họ và tên không được chứa ký tự đặc biệt. Chỉ cho phép chữ cái, khoảng trắng, dấu nháy đơn và dấu gạch ngang")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ! Vui lòng nhập đúng định dạng email (ví dụ: example@email.com)")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu bắt buộc")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string Password { get; set; }

        // Sửa: Thêm Required và Compare để kiểm tra xác nhận mật khẩu
        [Required(ErrorMessage = "Xác nhận mật khẩu bắt buộc")]
        [Compare("Password", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp")]
        public string ConfirmPassword { get; set; } = null!;

        [Required(ErrorMessage = "Bạn cần đồng ý với điều khoản")]
        public bool AgreeToTerms { get; set; }
    }
}