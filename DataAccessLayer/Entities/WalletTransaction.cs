using DataAccessLayer.Entities.Common;
using DataAccessLayer.Enums;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class WalletTransaction : BaseEntity<Guid>
{
    public Guid WalletId { get; set; }

    public string Type { get; set; } = null!;

    public decimal Amount { get; set; }

    public Guid? OrderId { get; set; }

    public string? ProviderTxnId { get; set; }

    public StatusValue Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Wallet Wallet { get; set; } = null!;
}
