using DataAccessLayer.Enums;
namespace PresentationLayer.Models
{
    public class CreateOrderViewModel
    {
        public AddressViewModel PickupAddress { get; set; }
        public AddressViewModel DropoffAddress { get; set; }
        public decimal? DistanceKm { get; set; }
        public int? EtaMinutes { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? PickupDate { get; set; }
        public string Note { get; set; }
        public string VehicleType { get; set; }

        // SỬA: Từ single value thành List
        public List<Guid> ServiceCategoryIds { get; set; }  // Thay đổi từ Guid? thành List<Guid>
        public List<int> ProductCategories { get; set; }     // Thay đổi từ ProductCategoryType? thành List<int>

        public decimal? EstimatedWeight { get; set; }
        public List<OrderItemViewModel> Items { get; set; }
    }

    public class OrderItemViewModel
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}