using PresentationLayer.Models;
using System.ComponentModel.DataAnnotations;

public class BookingRequestVM
{
    [Required(ErrorMessage = "Chọn địa chỉ giao hàng")]
    public Guid? DropoffAddressId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime StorageStartDate { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime StorageEndDate { get; set; }

    [MaxLength(200)]
    public string? DesiredWarehouseLocation { get; set; }

    public double? DropoffLatitude { get; set; }
    public double? DropoffLongitude { get; set; }
    public string? DropoffAddressText { get; set; }

    [MaxLength(500)]
    public string? Note { get; set; }

    public List<AddressOptionVM> AddressOptions { get; set; } = new();
    public List<BookingItemVM> Items { get; set; } = new();
    public List<string> SpecialRequirements { get; set; } = new();
}