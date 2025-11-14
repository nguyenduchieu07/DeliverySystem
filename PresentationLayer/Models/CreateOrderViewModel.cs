using System.ComponentModel.DataAnnotations;
using DataAccessLayer.Enums;
namespace PresentationLayer.Models
{
    public class CreateWarehouseOrderViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "Địa chỉ nhận hàng là bắt buộc")]
        public AddressViewModel PickupAddress { get; set; } = new();
        
        [Required(ErrorMessage = "Khu vực tìm kho là bắt buộc")]
        public AddressViewModel WarehouseArea { get; set; } = new();
        
        [Required(ErrorMessage = "Vui lòng chọn kho từ danh sách")]
        public Guid? WarehouseId { get; set; } // ID của warehouse đã chọn
        
        [Required(ErrorMessage = "Ngày gửi vào là bắt buộc")]
        [DataType(DataType.Date)]
        public DateTime StorageStartDate { get; set; }
        
        [Required(ErrorMessage = "Ngày lấy ra là bắt buộc")]
        [DataType(DataType.Date)]
        public DateTime StorageEndDate { get; set; }
        
        [Required(ErrorMessage = "Họ và tên là bắt buộc")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Họ và tên phải có từ 2 đến 100 ký tự")]
        public string CustomerFullName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [RegularExpression(@"^(\+84|0)[0-9]{9,10}$", ErrorMessage = "Số điện thoại không hợp lệ! Vui lòng nhập số điện thoại 10 chữ số (ví dụ: 0912345678 hoặc +84912345678)")]
        public string CustomerPhone { get; set; } = string.Empty;
        
        [EmailAddress(ErrorMessage = "Email không hợp lệ! Vui lòng nhập đúng định dạng email (ví dụ: example@email.com)")]
        public string? CustomerEmail { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập ít nhất một món đồ cần lưu kho")]
        [MinLength(1, ErrorMessage = "Vui lòng nhập ít nhất một món đồ cần lưu kho")]
        public List<OrderItemViewModel> Items { get; set; } = new();
        
        public List<string> SpecialRequirements { get; set; } = new();
        public string? Note { get; set; }
        public string? ProductImageUrl { get; set; } // URL ảnh tổng của toàn bộ sản phẩm
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            
            // Validate ngày tháng
            if (StorageStartDate < DateTime.Today)
            {
                results.Add(new ValidationResult(
                    "Ngày gửi vào không được là ngày trong quá khứ!",
                    new[] { nameof(StorageStartDate) }));
            }
            
            if (StorageEndDate <= StorageStartDate)
            {
                results.Add(new ValidationResult(
                    "Ngày lấy ra phải sau ngày gửi vào!",
                    new[] { nameof(StorageEndDate) }));
            }
            
            // Validate địa chỉ
            if (PickupAddress != null && string.IsNullOrWhiteSpace(PickupAddress.AddressLine))
            {
                results.Add(new ValidationResult(
                    "Địa chỉ nhận hàng không được để trống. Vui lòng nhập địa chỉ hoặc chọn vị trí hiện tại.",
                    new[] { nameof(PickupAddress) }));
            }
            
            if (WarehouseArea != null && string.IsNullOrWhiteSpace(WarehouseArea.AddressLine))
            {
                results.Add(new ValidationResult(
                    "Khu vực tìm kho không được để trống. Vui lòng chọn kho từ danh sách.",
                    new[] { nameof(WarehouseArea) }));
            }
            
            // Validate items
            if (Items == null || !Items.Any())
            {
                results.Add(new ValidationResult(
                    "Vui lòng nhập ít nhất một món đồ cần lưu kho!",
                    new[] { nameof(Items) }));
            }
            else
            {
                var validItems = Items.Where(i => !string.IsNullOrWhiteSpace(i.Name) && i.Quantity > 0).ToList();
                if (!validItems.Any())
                {
                    results.Add(new ValidationResult(
                        "Vui lòng nhập ít nhất một món đồ cần lưu kho!",
                        new[] { nameof(Items) }));
                }
                
                // Validate từng item
                for (int i = 0; i < Items.Count; i++)
                {
                    var item = Items[i];
                    if (!string.IsNullOrWhiteSpace(item.Name) || item.Quantity > 0)
                    {
                        if (string.IsNullOrWhiteSpace(item.Name))
                        {
                            results.Add(new ValidationResult(
                                $"Món đồ thứ {i + 1}: Vui lòng nhập tên đồ dùng!",
                                new[] { $"{nameof(Items)}[{i}].{nameof(OrderItemViewModel.Name)}" }));
                        }
                        else if (item.Name.Length < 2)
                        {
                            results.Add(new ValidationResult(
                                $"Món đồ thứ {i + 1}: Tên đồ dùng phải có ít nhất 2 ký tự!",
                                new[] { $"{nameof(Items)}[{i}].{nameof(OrderItemViewModel.Name)}" }));
                        }
                        
                        if (item.Quantity <= 0)
                        {
                            results.Add(new ValidationResult(
                                $"Món đồ \"{item.Name}\": Số lượng phải lớn hơn 0!",
                                new[] { $"{nameof(Items)}[{i}].{nameof(OrderItemViewModel.Quantity)}" }));
                        }
                        else if (item.Quantity > 10000)
                        {
                            results.Add(new ValidationResult(
                                $"Món đồ \"{item.Name}\": Số lượng không được vượt quá 10,000!",
                                new[] { $"{nameof(Items)}[{i}].{nameof(OrderItemViewModel.Quantity)}" }));
                        }
                    }
                }
            }
            
            return results;
        }
    }

    public class OrderItemViewModel
    {
        [Required(ErrorMessage = "Tên đồ dùng là bắt buộc")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Tên đồ dùng phải có từ 2 đến 200 ký tự")]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(50, ErrorMessage = "Danh mục không được vượt quá 50 ký tự")]
        public string? Category { get; set; }
        
        [Required(ErrorMessage = "Số lượng là bắt buộc")]
        [Range(1, 10000, ErrorMessage = "Số lượng phải từ 1 đến 10,000")]
        public int Quantity { get; set; }
    }
}