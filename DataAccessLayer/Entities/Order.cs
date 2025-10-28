using DataAccessLayer.Entities.Common;
using DataAccessLayer.Enums;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities
{
    public partial class Order : BaseEntity<Guid>
    {
        public Guid CustomerId { get; set; }
        public Guid StoreId { get; set; }
        public Guid? QuotationId { get; set; }
        public Guid? PickupAddressId { get; set; }
        public Guid? DropoffAddressId { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? PickupDate { get; set; }
        public decimal? DistanceKm { get; set; }
        public int? EtaMinutes { get; set; }
        public StatusValue Status { get; set; }
        /*
         * Draft        = Yêu cầu báo giá từ Customer
         * Pending      = Báo giá từ Store (chờ Customer)
         * Approved     = Đơn hàng được chấp nhận
         * Rejected     = Báo giá bị từ chối
         * Completed    = Đơn hàng hoàn thành
         */
        public decimal TotalAmount { get; set; }
        public string? Note { get; set; }
        public string? StoreNote { get; set; }
        public int? EstimatedDays { get; set; }
        public DateTime? ValidUntil { get; set; }
        public DateTime? ResponseDeadline { get; set; }
        public virtual Order? ParentOrder { get; set; }
        public virtual ICollection<Order> ChildOrders { get; set; } = new List<Order>();
        public Guid? ParentOrderId { get; set; }

        // ✅ THÊM: Lưu ProductCategories dạng JSON
        public string? ProductCategoryIds { get; set; } // JSON: [1,2,3]

        // Navigation properties
        public virtual Customer Customer { get; set; } = null!;
        public virtual Address? DropoffAddress { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual Address? PickupAddress { get; set; }
        public virtual Quotation? Quotation { get; set; }
        public virtual Store Store { get; set; } = null!;
        public virtual ICollection<WalletTransaction> WalletTransactions { get; set; } = new List<WalletTransaction>();
    }
}