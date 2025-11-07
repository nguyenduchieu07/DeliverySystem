using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities.Common;
using DataAccessLayer.Enums;

namespace DataAccessLayer.Entities
{
    public partial class ItemReport : BaseEntity<Guid>
    {
        // public Guid OrderId { get; set; }
        public Guid OrderItemId { get; set; }
        public ItemReportType Type { get; set; }  // Hư hỏng / Sai mô tả / Thiếu hàng
        public int Quantity { get; set; } // Số lượng hàng thực tế nhận
        public string? ConditionNote { get; set; } // Ghi chú tình trạng hàng
        public string? Description { get; set; }        // Mô tả chi tiết sự cố
        public string? ImageUrl { get; set; }           // Ảnh minh chứng
        public ReportStatus Status { get; set; } = ReportStatus.Pending; // Chờ xử lý, Đã xử lý
        public DateTime ReportedAt { get; set; } = DateTime.Now;

        // Nếu có bồi thường / hoàn hàng
        public bool IsReturned { get; set; } = false;
        public bool IsCompensated { get; set; } = false;
        public decimal? CompensationAmount { get; set; }

        // Navigation
        // public virtual Order Order { get; set; } = null!;
        public virtual OrderItem OrderItem { get; set; } = null!;
        public virtual ICollection<ItemReportAction> Actions { get; set; } = new List<ItemReportAction>();
    }

}
