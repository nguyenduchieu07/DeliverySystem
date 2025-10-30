using DataAccessLayer.Entities.Common;
using DataAccessLayer.Enums;


namespace DataAccessLayer.Entities
{
    public class Contract : BaseEntity<Guid>
    {
        public Guid QuotationId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid StoreId { get; set; }
        public Guid WarehouseId { get; set; }
        public Guid? WarehouseSlotId { get; set; }

        // Tổng giá trị hợp đồng (từ báo giá)
        public decimal TotalAmount { get; set; }

        // Thời hạn hợp đồng
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // File PDF đã sinh
        public string? PdfUrl { get; set; }

        // Trạng thái hợp đồng
        public ContractStatus Status { get; set; } // Active / Expired / Terminating / Terminated

        // Ghi chú, điều khoản đặc biệt, v.v.
        public string? TermsAndConditions { get; set; }

        // Navigation
        public virtual Quotation Quotation { get; set; } = null!;
        public virtual Customer Customer { get; set; } = null!;
        public virtual Store Store { get; set; } = null!;
        public virtual Warehouse Warehouse { get; set; } = null!;
        public virtual WarehouseSlot? WarehouseSlot { get; set; }
    }
}