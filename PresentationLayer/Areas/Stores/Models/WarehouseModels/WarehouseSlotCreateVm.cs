using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Areas.Stores.Models.WarehouseModels
{
    public class WarehouseSlotCreateVm
    {
        [Required]
        public Guid WarehouseId { get; set; }

        [Required]
        public string Code { get; set; } = null!;

        [Required]
        public string Status { get; set; } = "Available";

        public string? Size { get; set; }

        [Display(Name = "Image")]
        public IFormFile? ImageFile { get; set; }
    }
}
