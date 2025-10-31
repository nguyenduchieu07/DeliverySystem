using DataAccessLayer.Enums;
namespace PresentationLayer.Models
{
    public class CreateWarehouseOrderViewModel
    {
        public AddressViewModel PickupAddress { get; set; } = new();
        public AddressViewModel WarehouseArea { get; set; } = new();
        public DateTime StorageStartDate { get; set; }
        public DateTime StorageEndDate { get; set; }
        public List<OrderItemViewModel> Items { get; set; } = new();
        public List<string> SpecialRequirements { get; set; } = new();
        public string? Note { get; set; }
    }

    public class OrderItemViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string? Category { get; set; }
        public int Quantity { get; set; }
        public decimal? EstimatedWeightKg { get; set; }
    }
}