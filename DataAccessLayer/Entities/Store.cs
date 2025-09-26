using DataAccessLayer.Entities.Common;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class Store : BaseEntity<Guid>
{
    public Guid OwnerUserId { get; set; }

    public string StoreName { get; set; } = null!;

    public string? LegalName { get; set; }

    public string? LicenseNumber { get; set; }

    public string? TaxNumber { get; set; }

    public string Status { get; set; } = null!;

    public string? KycLevel { get; set; }

    public decimal RatingAvg { get; set; }

    public int RatingCount { get; set; }
    public string? KycStatus { get; set; }        // Pending | NeedChanges | Approved | Rejected
    public string? KycNote { get; set; }          // ghi chú admin / yêu cầu bổ sung
    public DateTime? KycSubmittedAt { get; set; }
    public DateTime? KycReviewedAt { get; set; }
    public Guid? KycReviewedBy { get; set; }

    // tuỳ chọn cấu hình vận hành sau khi duyệt
    public int? MaxOrdersPerDay { get; set; }
    public string? ActiveRegions { get; set; }
    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User OwnerUser { get; set; } = null!;

    public virtual ICollection<Quotation> Quotations { get; set; } = new List<Quotation>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

    public virtual ICollection<StoreStaff> StoreStaffs { get; set; } = new List<StoreStaff>();

    public virtual ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
}
