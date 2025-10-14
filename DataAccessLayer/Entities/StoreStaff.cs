using DataAccessLayer.Entities.Common;
using DataAccessLayer.Enums;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class StoreStaff : BaseEntity<Guid>
{

    public Guid StoreId { get; set; }

    public Guid UserId { get; set; }

    public string Role { get; set; } = null!;

    public StatusValue Status { get; set; }

    public virtual Store Store { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
