using DataAccessLayer.Entities.Common;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class StoreStaff : BaseEntity<Guid>
{

    public Guid StoreId { get; set; }

    public Guid UserId { get; set; }

    public string Role { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
