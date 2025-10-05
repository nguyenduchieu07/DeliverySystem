using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models
{
    public class BookingRequestVM
    {
        [Required(ErrorMessage = "Chọn địa chỉ lấy hàng")]
        public Guid? PickupAddressId { get; set; }
        [Required(ErrorMessage = "Chọn địa chỉ giao hàng")]
        public Guid? DropoffAddressId { get; set; }


        [Required]
        [DataType(DataType.Date)]
        public DateTime PickupDate { get; set; }


        [Required]
        [Display(Name = "Giờ lấy hàng")]
        public string PickupTime { get; set; } = "09:00"; // HH:mm


        [MaxLength(500)]
        public string? Note { get; set; }


        public List<AddressOptionVM> AddressOptions { get; set; } = new();
        public List<BookingItemVM> Items { get; set; } = new();
    }
}
