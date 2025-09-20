using DataAccessLayer.Entities.Common;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class Wallet : BaseEntity<Guid>
{
    public string OwnerType { get; set; } = null!;

    public Guid OwnerId { get; set; }

    public string Currency { get; set; } = null!;

    public decimal Balance { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<WalletTransaction> WalletTransactions { get; set; } = new List<WalletTransaction>();
}
