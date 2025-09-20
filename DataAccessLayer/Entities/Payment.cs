using DataAccessLayer.Entities.Common;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class Payment : BaseEntity<Guid>
{
    public Guid OrderId { get; set; }

    public decimal Amount { get; set; }

    public string Method { get; set; } = null!;

    public string? Provider { get; set; }

    public string? ProviderTxnId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Order Order { get; set; } = null!;
}
