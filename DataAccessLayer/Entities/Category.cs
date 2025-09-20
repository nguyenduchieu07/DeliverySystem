using DataAccessLayer.Entities.Common;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class Category : BaseEntity<Guid>
{
    public Guid? ParentId { get; set; }

    public string Name { get; set; } = null!;

    public string? Slug { get; set; }

    public int? SortOrder { get; set; }

    public virtual ICollection<Category> InverseParent { get; set; } = new List<Category>();

    public virtual Category? Parent { get; set; }

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
