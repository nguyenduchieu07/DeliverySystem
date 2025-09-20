using DataAccessLayer.Entities.Common;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class Order : BaseEntity<Guid>
{
    public Guid CustomerId { get; set; }

    public Guid StoreId { get; set; }

    public Guid? QuotationId { get; set; }

    public Guid? PickupAddressId { get; set; }

    public Guid? DropoffAddressId { get; set; }

    public decimal? DistanceKm { get; set; }

    public int? EtaMinutes { get; set; }

    public string Status { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public string? Note { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

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
