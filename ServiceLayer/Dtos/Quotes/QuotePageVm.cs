

namespace ServiceLayer.Dtos.Quotes
{
    public class QuotePageVm
    {
        public int FreeDays { get; set; } = 2;
        public decimal VatRate { get; set; } = 0.10m;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class QuoteRequestVm
    {
        public Guid StoreId { get; set; }
        public Guid WarehouseId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Guid> SlotIds { get; set; } = new();
        public List<AddonVm> Addons { get; set; } = new();
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
    }

    public class AddonVm
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public bool IsPerM3 { get; set; }
    }

    public class QuoteResultVm
    {
        public Guid QuotationId { get; set; }
        public string QuoteCode { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime ValidUntil { get; set; }

        public WarehouseInfoVm Warehouse { get; set; } = new();
        public CustomerInfoVm Customer { get; set; } = new();

        public List<SlotDetailVm> Slots { get; set; } = new();
        public List<AddonCalculationVm> Addons { get; set; } = new();

        public int TotalDays { get; set; }
        public int ChargeableDays { get; set; }
        public int Hours { get; set; }

        public decimal TotalSlotFee { get; set; }
        public decimal TotalAddonFee { get; set; }
        public decimal Subtotal { get; set; }
        public decimal VatAmount { get; set; }
        public decimal Total { get; set; }

        public decimal? DistanceKm { get; set; }
    }

    public class WarehouseInfoVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }

    public class CustomerInfoVm
    {
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }

    public class SlotDetailVm
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public decimal VolumeM3 { get; set; }
        public decimal LengthM { get; set; }
        public decimal WidthM { get; set; }
        public decimal HeightM { get; set; }
        public decimal PricePerHour { get; set; }
        public decimal TotalFee { get; set; }
    }

    public class AddonCalculationVm
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }

    public class HoldTempVm
    {
        public Guid QuotationId { get; set; }
        public List<Guid> SlotIds { get; set; } = new();
        public int HoldMinutes { get; set; } = 120;
    }

    public class AcceptQuoteVm
    {
        public Guid QuotationId { get; set; }
    }

    public class RequestRevisionVm
    {
        public Guid QuotationId { get; set; }
        public string Note { get; set; } = string.Empty;
    }
}