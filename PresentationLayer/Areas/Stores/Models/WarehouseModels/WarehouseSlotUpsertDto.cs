using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Areas.Stores.Models.WarehouseModels
{
    public class WarehouseSlotUpsertDto
    {
        public Guid? Id { get; set; } // null => Create

        [Required] public Guid WarehouseId { get; set; }

        [Required, StringLength(50)]
        public string Code { get; set; } = null!;

        // Status là string → dùng select với giá trị chữ
        [Required, StringLength(30)]
        public string Status { get; set; } = "Available"; // Available / Reserved / Occupied / Maintenance

        public string? Size { get; set; } // nullable theo entity

        // Upload ảnh (tùy chọn). Nếu up mới khi Edit → thay ảnh cũ.
        public IFormFile? ImageFile { get; set; }
        public string? ExistingImageUrl { get; set; } // hiển thị preview khi edit
    }
}
