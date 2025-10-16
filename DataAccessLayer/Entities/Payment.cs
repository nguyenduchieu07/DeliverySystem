using DataAccessLayer.Entities.Common;
using DataAccessLayer.Enums;
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

    public StatusValue Status { get; set; }
    public virtual Order Order { get; set; } = null!;
}
