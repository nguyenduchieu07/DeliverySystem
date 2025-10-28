using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos.Quotes
{
    public class QuotationListDto
    {
        public Guid Id { get; set; }
        public string StoreName { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime ValidUntil { get; set; }
        public StatusValue Status { get; set; }
        public DateTime CreatedAt { get; set; }

        // Thông tin store
        public decimal StoreRatingAvg { get; set; }
        public int StoreRatingCount { get; set; }
        public string StoreContactPhone { get; set; }

        // Số lượng dịch vụ
        public int ServiceCount { get; set; }

        // Trạng thái hiển thị
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
    }
}
