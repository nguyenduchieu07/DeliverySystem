using System.ComponentModel.DataAnnotations;

public class BookingItemVM
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Category { get; set; }

    [Range(0, 100000, ErrorMessage = "Trọng lượng không hợp lệ")]
    public decimal? EstimatedWeightKg { get; set; }

    [Range(1, 100000, ErrorMessage = "Số lượng >= 1")]
    public int Quantity { get; set; } = 1;

    // URL ảnh từ Cloudinary
    public string? ImageUrl { get; set; }
}