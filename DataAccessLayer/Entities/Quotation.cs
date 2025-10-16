using DataAccessLayer.Entities.Common;
using DataAccessLayer.Enums;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class Quotation : BaseEntity<Guid>
{
    public Guid StoreId { get; set; }

    public Guid CustomerId { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime ValidUntil { get; set; }

    public StatusValue Status { get; set; }
    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Store Store { get; set; } = null!;
}
