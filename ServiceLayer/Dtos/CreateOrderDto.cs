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
        public List<OrderItemDto> Items { get; set; }
    }

    public class AddressDto
    {
        public string AddressLine { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class OrderItemDto
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}