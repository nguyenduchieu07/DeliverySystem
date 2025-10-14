using DataAccessLayer.Entities.Common;
using DataAccessLayer.Enums;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class Service : BaseEntity<Guid>
{
    public Guid StoreId { get; set; }

    public Guid? CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string Unit { get; set; } = null!;

    public decimal BasePrice { get; set; }
    // NEW: mô hình tính giá mặc định của dịch vụ
    public PricingModel PricingModel { get; set; } = PricingModel.DimensionBased;
    public bool IsActive { get; set; }
    public StatusValue Status {  get; set; }
    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<ServicePriceRule> PriceRules { get; set; } = new List<ServicePriceRule>();
    public virtual ICollection<ServiceAddon> Addons { get; set; } = new List<ServiceAddon>();

    // NEW: preset size S/M/L/XL...
    public virtual ICollection<ServiceSizeOption> SizeOptions { get; set; } = new List<ServiceSizeOption>();
    public virtual Store Store { get; set; } = null!;
}
