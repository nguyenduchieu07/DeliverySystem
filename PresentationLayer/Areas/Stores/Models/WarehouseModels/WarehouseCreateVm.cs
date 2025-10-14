using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Areas.Stores.Models.WarehouseModels
{
    public class WarehouseCreateVm
    {

        [ScaffoldColumn(false)]
        public Guid StoreId { get; set; }   // set ở Controller, không cho user chọn

        [Required, StringLength(200)]
        public string Name { get; set; } = null!;

        [Required]
        public string AddressLine { get; set; } = null!;
        public string? Ward { get; set; }
        public string? District { get; set; }
        public string? City { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        [Display(Name = "Cover image")]
        public IFormFile? CoverImageFile { get; set; }

        [Display(Name = "Map image")]
        public IFormFile? MapImageFile { get; set; }
    }
}
