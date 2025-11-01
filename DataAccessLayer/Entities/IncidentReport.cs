using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities.Common;
using DataAccessLayer.Enums;

namespace DataAccessLayer.Entities
{
    public partial class IncidentReport : BaseEntity<Guid>
    {
        public Guid OrderId { get; set; }
        public Guid OrderItemId { get; set; }
        public IncidentType IncidentType { get; set; }  // Hư hỏng / Sai mô tả / Thiếu hàng
        public string? Description { get; set; }        // Mô tả chi tiết sự cố
        public string? ImageUrl { get; set; }           // Ảnh minh chứng
        public ReportStatus Status { get; set; } = ReportStatus.Pending; // Chờ xử lý, Đã xử lý
        public DateTime ReportedAt { get; set; } = DateTime.UtcNow;

        // Nếu có bồi thường / hoàn hàng
        public bool IsReturned { get; set; } = false;
        public bool IsCompensated { get; set; } = false;
        public decimal? CompensationAmount { get; set; }

        // Navigation
        public virtual Order Order { get; set; } = null!;
        public virtual OrderItem OrderItem { get; set; } = null!;
        public virtual ICollection<IncidentAction> Actions { get; set; } = new List<IncidentAction>();
    }

}
