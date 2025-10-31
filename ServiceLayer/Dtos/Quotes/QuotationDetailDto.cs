using DataAccessLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos.Quotes
{
    public class QuotationDetailDto
    {
        public Guid Id { get; set; }
        public Guid? StoreId { get; set; }
        public Guid CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime ValidUntil { get; set; }
        public StatusValue Status { get; set; }
        public DateTime CreatedAt { get; set; }

        // Thông tin Store
        public string StoreName { get; set; }
        public string StoreContactPhone { get; set; }
        public string StoreContactEmail { get; set; }
        public decimal StoreRatingAvg { get; set; }
        public int StoreRatingCount { get; set; }
        public string StoreAddress { get; set; }

        // Thông tin Customer
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }

        // Danh sách dịch vụ trong báo giá
        public List<QuotationServiceDto> Services { get; set; } = new();

        // Thông tin đơn hàng liên quan (nếu có)
        public Guid? RelatedOrderId { get; set; }
        public string OrderNote { get; set; }

        public string StatusDisplay => Status switch
        {
            StatusValue.Pending => "Chờ xử lý",
            StatusValue.Approved => "Đã chấp nhận",
            StatusValue.Rejected => "Đã từ chối",
            StatusValue.Expired => "Hết hạn",
            _ => "Không xác định"
        };

        public bool IsExpired => DateTime.Now > ValidUntil;
        public bool CanAccept => Status == StatusValue.Pending && !IsExpired;
        public bool CanReject => Status == StatusValue.Pending && !IsExpired;
    }
}
