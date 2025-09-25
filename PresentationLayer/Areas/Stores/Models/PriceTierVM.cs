using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Areas.Stores.Models
{
    public class PriceTierVM
    {
        public Guid? Id { get; set; }
        public Guid ServiceId { get; set; }
        [Required] public DateTime ValidFrom { get; set; } = DateTime.UtcNow.Date;
        public DateTime? ValidTo { get; set; }
        [Range(0.01, 999999999)] public decimal Price { get; set; }
        public int? MinQty { get; set; }
        public int? MaxQty { get; set; }
    }
}
