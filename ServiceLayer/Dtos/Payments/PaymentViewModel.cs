using DataAccessLayer.Enums;
using System.ComponentModel.DataAnnotations;

public class PaymentViewModel
{
    public Guid OrderId { get; set; }

    [Display(Name = "Phương thức thanh toán")]
    [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán")]
    public string PaymentMethod { get; set; } = null!;

    [Display(Name = "Nhà cung cấp")]
    public string? Provider { get; set; }

    public decimal TotalAmount { get; set; }

    public string? Note { get; set; }

    // Thông tin đơn hàng để hiển thị
    public OrderSummaryViewModel OrderSummary { get; set; } = null!;
}

public class OrderSummaryViewModel
{
    public Guid OrderId { get; set; }
    public string CustomerName { get; set; } = null!;
    public string CustomerPhone { get; set; } = null!;
    public string? PickupAddress { get; set; }
    public string? DropoffAddress { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DateTime? PickupDate { get; set; }
    public decimal? DistanceKm { get; set; }
    public int? EtaMinutes { get; set; }
    public decimal TotalAmount { get; set; }
    public StatusValue Status { get; set; }
    public string? Note { get; set; }
    public List<OrderItemSummaryViewModel> OrderItems { get; set; } = new List<OrderItemSummaryViewModel>();
}

public class OrderItemSummaryViewModel
{
    public string? ItemName { get; set; }
    public string? Description { get; set; }
    public int Quantity { get; set; }
    public decimal? LengthM { get; set; }
    public decimal? WidthM { get; set; }
    public decimal? HeightM { get; set; }
    public decimal? WeightKg { get; set; }
    public string? SizeCode { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? Subtotal { get; set; }

    public string DisplaySize =>
        LengthM.HasValue && WidthM.HasValue && HeightM.HasValue
            ? $"{LengthM}m x {WidthM}m x {HeightM}m"
            : SizeCode ?? "Chưa xác định";
}

public class PaymentResultViewModel
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = null!;
    public Guid? PaymentId { get; set; }
    public string? TransactionId { get; set; }
    public string? RedirectUrl { get; set; }
}