using DataAccessLayer.Entities.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class User : IdentityUser<Guid>
{
    public string Status { get; set; } = "Active";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<StoreStaff> StoreStaffs { get; set; } = new List<StoreStaff>();

    public virtual ICollection<Store> Stores { get; set; } = new List<Store>();
}
