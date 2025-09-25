using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Areas.Stores.Models
{
    public class ServiceEditVM
    {
        public Guid? Id { get; set; }
        [Required, StringLength(200)] public string Name { get; set; } = string.Empty;
        [StringLength(1000)] public string? Description { get; set; }
        [Required, StringLength(20)] public string Unit { get; set; } = "gói";
        [Range(0, 999999999)] public decimal BasePrice { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsPublished { get; set; } = false;
        public Guid StoreId { get; set; }

        // Category binding
        [Display(Name = "Danh mục cha")] public Guid? ParentCategoryId { get; set; }
        [Display(Name = "Danh mục con"), Required(ErrorMessage = "Hãy chọn danh mục con")] public Guid? CategoryId { get; set; }

        public List<PriceTierVM> Tiers { get; set; } = new();
    }
}
