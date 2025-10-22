using DataAccessLayer.Entities.Common;
using System;

namespace DataAccessLayer.Entities
{
    public partial class OrderItem : BaseEntity<Guid>
    {
        public Guid OrderId { get; set; }
        public Guid? ServiceId { get; set; }

        // Thông tin mô tả hàng hóa
        public string? ItemName { get; set; }

        public string? Description { get; set; }
        public int Quantity { get; set; }

        // Kích thước vật lý (nếu có)
        public decimal? LengthM { get; set; }
        public decimal? WidthM { get; set; }
        public decimal? HeightM { get; set; }
        public decimal? WeightKg { get; set; }

        // Tùy chọn: hệ thống định lượng theo size code (nếu bạn không cần nhập kích thước thật)
        public string? SizeCode { get; set; }

        // Tính toán nhanh thể tích (không lưu vào DB, chỉ get)
        public decimal? VolumeM3 => LengthM.HasValue && WidthM.HasValue && HeightM.HasValue
            ? LengthM * WidthM * HeightM
            : null;

        // Giá (nếu có, thường chưa có ở bước tạo order, vì kho báo giá sau)
        public decimal? UnitPrice { get; set; }
        public decimal? Subtotal { get; set; }

        // Navigation
        public virtual Order Order { get; set; } = null!;
        public virtual Service? Service { get; set; }
    }
}
