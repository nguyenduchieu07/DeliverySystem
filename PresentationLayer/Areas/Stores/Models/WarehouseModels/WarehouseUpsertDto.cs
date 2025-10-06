using DataAccessLayer.Entities;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Areas.Stores.Models.WarehouseModels
{
    public class WarehouseUpsertDto
    {
        public Guid? Id { get; set; } // null => Create

        [Required] public Guid StoreId { get; set; }
        [Required, StringLength(200)] public string Name { get; set; } = null!;

        // Chọn Address có sẵn
        public Guid? AddressRefId { get; set; }

        // Hoặc tạo Address mới từ bản đồ (nếu Address của bạn có các field này)
        public string? AddressLine { get; set; } // sẽ map vào Address.FullAddress (nếu tạo mới)
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        [Url] public string? CoverImageUrl { get; set; }   // ảnh bìa
        [Url] public string? MapImageUrl { get; set; }
        public IFormFile? CoverImage { get; set; }

        public IFormFile? MapImage { get; set; }
        public virtual ICollection<WarehouseSlot> Slots { get; set; } = new List<WarehouseSlot>();
    }
}
