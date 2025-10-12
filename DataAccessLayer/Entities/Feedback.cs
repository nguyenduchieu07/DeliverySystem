using DataAccessLayer.Entities.Common;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class Feedback : BaseEntity<Guid>
{
    public Guid OrderId { get; set; }

    public Guid FromUserId { get; set; }

    public Guid ToStoreId { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public virtual User FromUser { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual Store ToStore { get; set; } = null!;
}
