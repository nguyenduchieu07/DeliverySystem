using DataAccessLayer.Entities.Common;
using DataAccessLayer.Enums;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class ServicePriceRule : BaseEntity<Guid>
{
    public Guid ServiceId { get; set; }

    // Hiệu lực
    public DateTime ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }

    // Điều kiện theo thể tích/diện tích (tuỳ use-case)
    public decimal? MinVolumeM3 { get; set; }
    public decimal? MaxVolumeM3 { get; set; }

    public decimal? MinAreaM2 { get; set; }
    public decimal? MaxAreaM2 { get; set; }

    // Điều kiện theo số lượng (nếu có)
    public decimal? MinQty { get; set; }
    public decimal? MaxQty { get; set; }

    // Điều kiện theo thời gian
    public int? MinDays { get; set; }
    public int? MaxDays { get; set; }
    public TimeUnit TimeUnit { get; set; } = TimeUnit.Day;

    // Giá áp dụng (ý nghĩa phụ thuộc ApplyModel)
    public decimal Price { get; set; }

    // Kiểu áp dụng cho rule này (ưu tiên rule level > service default)
    public PricingModel ApplyModel { get; set; } = PricingModel.DimensionBased;

    public virtual Service Service { get; set; } = null!;
}
