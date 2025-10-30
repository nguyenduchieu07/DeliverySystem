using DataAccessLayer.Entities.Common;
using DataAccessLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class WarehouseSlot : BaseEntity<Guid>
    {
        public Guid WarehouseId { get; set; }
        public string Code { get; set; } = null!;
        public string? Size { get; set; }
        public string? ImageUrl { get; set; }
        public Guid? CurrentOrderId { get; set; }
        public StatusValue Status { get; set; }

        public int Row { get; set; } // bắt đầu từ 1
        public int Col { get; set; } // bắt đầu từ 1

        public decimal HeightM { get; set; } // Cao
        public decimal LengthM { get; set; } // Dài
        public decimal WidthM { get; set; } // Rộng

        public decimal VolumeM3 => Math.Round(HeightM * LengthM * WidthM, 3);

        // Thuê & cảnh báo
        public DateTime? LeaseStart { get; set; }
        public DateTime? LeaseEnd { get; set; }
        public bool IsBlocked { get; set; } // chỗ không dùng / để trống

        // Giá cơ bản (VND/giờ). Có thể kết hợp bảng PriceRule để setting theo thời gian
        public decimal BasePricePerHour { get; set; }
        public virtual Warehouse Warehouse { get; set; } = null!;
        
    }
}