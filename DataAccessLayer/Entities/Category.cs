using DataAccessLayer.Entities.Common;
using DataAccessLayer.Enums;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class Category : BaseEntity<Guid>
{
    public Guid? StoreId { get; set; } // null = global category

    public Guid? ParentId { get; set; }

    public string Name { get; set; } = null!;
    public string? Slug { get; set; }    // unique (global) hoặc unique theo StoreId
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public string? ThumbnailUrl { get; set; }

    public int? SortOrder { get; set; }

    // Tối ưu cây:
    public string? Path { get; set; }    // ví dụ: "/luu-kho/luu-kho-lanh"
    public int Level { get; set; }       // root=0
    public bool IsLeaf { get; set; }     // true nếu không có con

    // Trạng thái:
    public bool IsActive { get; set; } = true;
    public StatusValue Status { get; set; } = StatusValue.Active;

    // Navs
    public virtual Category? Parent { get; set; }
    public virtual ICollection<Category> InverseParent { get; set; } = new List<Category>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

    // (Tùy chọn) nếu có bảng Store:
    public virtual Store? Store { get; set; }
}
