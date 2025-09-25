namespace PresentationLayer.Areas.Stores.Models
{
    public class ServiceListItemVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? CategoryName { get; set; }
        public string Unit { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public bool IsActive { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
