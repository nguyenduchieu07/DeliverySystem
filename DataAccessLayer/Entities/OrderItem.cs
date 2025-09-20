using DataAccessLayer.Entities.Common;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class OrderItem : BaseEntity<Guid>
{
    public Guid OrderId { get; set; }

    public Guid ServiceId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal Subtotal { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
