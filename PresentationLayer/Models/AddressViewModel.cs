namespace PresentationLayer.Models
{
    public class AddressViewModel
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? StoreId { get; set; }
        public string? Label { get; set; }
        public string AddressLine { get; set; } = null!;
        public string? Ward { get; set; }
        public string? District { get; set; }
        public string? City { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public bool IsDefault { get; set; }
        public bool Active { get; set; }
        public string RecipientName { get; set; }
        public string RecipientPhone { get; set; }
        public string Floor { get; set; }
    }
}