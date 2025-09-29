using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models
{
    public class BookingItemVM
    {
        [Required]
        public Guid ServiceId { get; set; }
        [Range(1, 100000, ErrorMessage = "Số lượng >= 1")]
        public int Quantity { get; set; } = 1;
    }
}
