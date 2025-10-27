// ServiceLayer/Services/QuotationService.cs
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Abstractions.IServices;
using ServiceLayer.Dtos.Quotes;

namespace ServiceLayer.Services
{
    public class QuotationService : IQuotationService
    {
        private readonly DeliverySytemContext _db;
        private const int FREE_DAYS = 2;
        private const decimal VAT_RATE = 0.10m;
        private const int VALIDITY_HOURS = 48;
        private readonly IUserContextService _context;
        public QuotationService(DeliverySytemContext db, IUserContextService context)
        {
            _db = db;
            _context = context;
        }

        public async Task<QuoteResultVm> CalculateAndCreateQuotationAsync(
            QuoteRequestVm req,
            CancellationToken ct)
        {
            // Load slots
            var slots = await _db.WarehouseSlots
                .Where(s => req.SlotIds.Contains(s.Id))
                .ToListAsync(ct);
            var userId = Guid.Parse(_context.GetUserId()!);

            // Calculate days
            var totalDays = (req.EndDate - req.StartDate).Days;
            var chargeableDays = Math.Max(0, totalDays - FREE_DAYS);
            var hours = chargeableDays * 24;

            // Calculate slot fees
            decimal totalSlotFee = 0;
            decimal totalVolume = 0;
            var slotDetails = new List<SlotDetailVm>();

            foreach (var slot in slots)
            {
                var slotFee = slot.BasePricePerHour * hours;
                totalSlotFee += slotFee;
                totalVolume += slot.VolumeM3;

                slotDetails.Add(new SlotDetailVm
                {
                    Id = slot.Id,
                    Code = slot.Code,
                    Size = slot.Size ?? "",
                    VolumeM3 = slot.VolumeM3,
                    LengthM = slot.LengthM,
                    WidthM = slot.WidthM,
                    HeightM = slot.HeightM,
                    PricePerHour = slot.BasePricePerHour,
                    TotalFee = slotFee
                });
            }

            // Calculate addons
            decimal totalAddonFee = 0;
            var addonCalculations = new List<AddonCalculationVm>();

            foreach (var addon in req.Addons)
            {
                decimal addonPrice = addon.IsPerM3
                    ? addon.Value * totalVolume * chargeableDays
                    : addon.Value;

                totalAddonFee += addonPrice;
                addonCalculations.Add(new AddonCalculationVm
                {
                    Name = addon.Name,
                    Price = addonPrice
                });
            }

            // Calculate totals
            var subtotal = totalSlotFee + totalAddonFee;
            var vatAmount = subtotal * VAT_RATE;
            var total = subtotal + vatAmount;

            // Create quotation
            var quotation = new Quotation
            {
                Id = Guid.NewGuid(),
                StoreId = req.StoreId,
                CustomerId = userId, // Get from authenticated user
                TotalAmount = total,
                ValidUntil = DateTime.UtcNow.AddHours(VALIDITY_HOURS),
                Status = StatusValue.Draft,
                CreatedAt = DateTime.UtcNow
            };

            _db.Quotations.Add(quotation);
            await _db.SaveChangesAsync(ct);

            // Load warehouse info
            var warehouse = await _db.Warehouses
                .Include(w => w.Address)
                .FirstOrDefaultAsync(w => w.Id == req.WarehouseId, ct);

            return new QuoteResultVm
            {
                QuotationId = quotation.Id,
                QuoteCode = $"QT-{DateTime.Now.Year}-{quotation.Id.ToString().Substring(0, 6).ToUpper()}",
                CreatedAt = quotation.CreatedAt,
                ValidUntil = quotation.ValidUntil,
                Warehouse = new WarehouseInfoVm
                {
                    Id = warehouse.Id,
                    Name = warehouse.Name,
                    Address = warehouse.Address?.AddressLine ?? "",
                    Latitude = warehouse.Address?.Latitude,
                    Longitude = warehouse.Address?.Longitude
                },
                Customer = new CustomerInfoVm
                {
                    Name = req.CustomerName,
                    Phone = req.CustomerPhone
                },
                Slots = slotDetails,
                Addons = addonCalculations,
                TotalDays = totalDays,
                ChargeableDays = chargeableDays,
                Hours = hours,
                TotalSlotFee = totalSlotFee,
                TotalAddonFee = totalAddonFee,
                Subtotal = subtotal,
                VatAmount = vatAmount,
                Total = total
            };
        }

        public async Task<bool> CreateTempReservationAsync(HoldTempVm vm, CancellationToken ct)
        {
            var slots = await _db.WarehouseSlots.Where(e => vm.SlotIds.Contains(e.Id)).ToListAsync();
            var quotation = await _db.Quotations.Where(e => e.Id == vm.QuotationId).FirstOrDefaultAsync();

            foreach(var slot in slots)
            {
                slot.Status = StatusValue.Blocked;
                slot.LeaseStart = DateTime.UtcNow;
                slot.LeaseEnd = DateTime.UtcNow.AddMinutes(vm.HoldMinutes);
            }
            quotation!.Status = StatusValue.Sent;
            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> AcceptQuotationAsync(AcceptQuoteVm vm, CancellationToken ct)
        {
            var quotation = await _db.Quotations.FindAsync(vm.QuotationId);
            if (quotation == null) return false;

            quotation.Status = StatusValue.Active; // Or whatever status means "accepted"
            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> RequestRevisionAsync(RequestRevisionVm vm, CancellationToken ct)
        {
            var quotation = await _db.Quotations.FindAsync(vm.QuotationId);
            if (quotation == null) return false;
            quotation.Status = StatusValue.Revised;
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}