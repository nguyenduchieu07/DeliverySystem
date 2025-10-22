namespace ServiceLayer.DTOs
{
    public class CreateOrderDto
    {
        public AddressDto PickupAddress { get; set; }
        public AddressDto DropoffAddress { get; set; }
        public decimal? DistanceKm { get; set; }
        public int? EtaMinutes { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? PickupDate { get; set; }
        public string Note { get; set; }

        // ✅ GIỮ LẠI: ProductCategories (loại hàng hóa)
        public List<int> ProductCategories { get; set; }
        public decimal? EstimatedWeight { get; set; }
        public List<OrderItemDto> Items { get; set; }

        // ❌ BỎ: ServiceCategoryIds
    }

    public class AddressDto
    {
        public string AddressLine { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string RecipientName { get; set; }
        public string RecipientPhone { get; set; }
        public int? Floor { get; set; }
    }

    public class OrderItemDto
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}