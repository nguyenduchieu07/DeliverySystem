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

        // Chỉ giữ ProductCategories
        public List<int> ProductCategories { get; set; }     // Loại hàng hóa

        public decimal? EstimatedWeight { get; set; }
        public List<OrderItemViewModel> Items { get; set; }
    }

    public class OrderItemViewModel
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}