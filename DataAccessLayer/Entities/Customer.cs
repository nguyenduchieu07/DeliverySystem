using DataAccessLayer.Entities.Common;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class Customer : BaseEntity<Guid>
{

    public string FullName { get; set; } = null!;

    public string? PreferredLang { get; set; }

    public string? Tier { get; set; }

    public string? KycLevel { get; set; }

    public virtual User CustomerNavigation { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Quotation> Quotations { get; set; } = new List<Quotation>();
}
