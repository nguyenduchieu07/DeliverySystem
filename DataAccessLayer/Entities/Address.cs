using DataAccessLayer.Entities.Common;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class Address : BaseEntity<Guid>
{
    public Guid? UserId { get; set; }

    public Guid? StoreId { get; set; }

    public string? Label { get; set; }

    public string AddressLine { get; set; } = null!;

    public string? Ward { get; set; }

    public string? District { get; set; }

    public string? City { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public bool IsDefault { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<Order> OrderDropoffAddresses { get; set; } = new List<Order>();

    public virtual ICollection<Order> OrderPickupAddresses { get; set; } = new List<Order>();

    public virtual Store? Store { get; set; }

    public virtual User? User { get; set; }
}
