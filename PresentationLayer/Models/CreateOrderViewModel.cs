using DataAccessLayer.Enums;
namespace PresentationLayer.Models
{
    public class CreateWarehouseOrderViewModel
    {
        public AddressViewModel PickupAddress { get; set; } = new();
        public AddressViewModel WarehouseArea { get; set; } = new();
        public Guid? WarehouseId { get; set; } // ID của warehouse đã chọn
        public DateTime StorageStartDate { get; set; }
        public DateTime StorageEndDate { get; set; }
        public List<OrderItemViewModel> Items { get; set; } = new();
        public List<string> SpecialRequirements { get; set; } = new();
        public string? Note { get; set; }
        public string? ProductImageUrl { get; set; } // URL ảnh tổng của toàn bộ sản phẩm
    }

    public class OrderItemViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string? Category { get; set; }
        public int Quantity { get; set; }
        public decimal? EstimatedWeightKg { get; set; }
    }
}