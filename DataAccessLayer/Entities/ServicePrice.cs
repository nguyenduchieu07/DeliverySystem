using DataAccessLayer.Entities.Common;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class ServicePrice : BaseEntity<Guid>
{
    public Guid ServiceId { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime ValidTo { get; set; }

    public decimal Price { get; set; }

    public int? MinQty { get; set; }

    public int? MaxQty { get; set; }

    public virtual Service Service { get; set; } = null!;
}
