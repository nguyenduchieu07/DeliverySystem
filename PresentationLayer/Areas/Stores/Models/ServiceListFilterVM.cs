namespace PresentationLayer.Areas.Stores.Models
{
    public class ServiceListFilterVM
    {
        public string? Q { get; set; }
        public Guid? CategoryId { get; set; }
        public string Status { get; set; } = "All";      // Active, Inactive, All
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
    }
}
