using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Areas.Stores.Models.Services
{
    public class ServiceEditViewModel
    {
        public Guid? Id { get; set; }
        public Guid StoreId { get; set; }
        public Guid? CategoryId { get; set; }
        [Required] public string Name { get; set; } = null!;
        public string? Description { get; set; }
        [Required] public string Unit { get; set; } = "slot";
        [Range(0, double.MaxValue)] public decimal BasePrice { get; set; }
        public PricingModel PricingModel { get; set; } = PricingModel.TimeBased;
        public bool IsActive { get; set; } = true;
        public List<ServiceAddonVM> Addons { get; set; } = new();
        public List<ServiceSizeOptionVM> SizeOptions { get; set; } = new();
        public List<ServicePriceRuleVM> PriceRules { get; set; } = new();
    }

    public class ServiceSizeOptionVM
    {
        public Guid? Id { get; set; }
        public string Code { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public decimal? VolumeM3 { get; set; }
        public decimal? AreaM2 { get; set; }
        public decimal? MaxWeightKg { get; set; }
        public decimal? PriceOverride { get; set; }

        public ServiceSizeOptionVM() { }
        public ServiceSizeOptionVM(ServiceSizeOption o)
        {
            Id = o.Id; Code = o.Code; DisplayName = o.DisplayName;
            VolumeM3 = o.VolumeM3; AreaM2 = o.AreaM2;
            MaxWeightKg = o.MaxWeightKg; PriceOverride = o.PriceOverride;
        }
    }

    public class ServicePriceRuleVM
    {
        public Guid? Id { get; set; }
        public DateTime ValidFrom { get; set; } = DateTime.Now;
        public DateTime? ValidTo { get; set; }
        public decimal? MinVolumeM3 { get; set; }
        public decimal? MaxVolumeM3 { get; set; }
        public decimal? MinAreaM2 { get; set; }
        public decimal? MaxAreaM2 { get; set; }
        public decimal Price { get; set; }
        public PricingModel ApplyModel { get; set; } = PricingModel.TimeBased;
        public TimeUnit TimeUnit { get; set; } = TimeUnit.Day;
        public int? MinDays { get; set; }
        public int? MaxDays { get; set; }
        public decimal? MinQty { get; set; }
        public decimal? MaxQty { get; set; }
        public ServicePriceRuleVM() { }
        public ServicePriceRuleVM(ServicePriceRule r)
        {
            Id = r.Id;
            ValidFrom = r.ValidFrom;
            ValidTo = r.ValidTo;
            MinVolumeM3 = r.MinVolumeM3;
            MaxVolumeM3 = r.MaxVolumeM3;
            MinAreaM2 = r.MinAreaM2;
            MaxAreaM2 = r.MaxAreaM2;
            Price = r.Price;
            ApplyModel = r.ApplyModel;
            TimeUnit = r.TimeUnit;
            MinDays = r.MinDays;
            MaxDays = r.MaxDays;
            MinQty = r.MinQty;
            MaxQty = r.MaxQty;
        }
    }
    public class ServiceAddonVM
    {
        public Guid? Id { get; set; }
        [Required, StringLength(150)] public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsPercentage { get; set; }
        [Range(0, double.MaxValue)] public decimal Value { get; set; }

        public ServiceAddonVM() { }
        public ServiceAddonVM(DataAccessLayer.Entities.ServiceAddon a)
        {
            Id = a.Id; Name = a.Name; Description = a.Description; IsPercentage = a.IsPercentage; Value = a.Value;
        }
    }

}
