// ServiceLayer/Services/QuotationService.cs

using DataAccessLayer.Abstractions.IRepositories;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.VariantTypes;
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
        private const int VALIDITY_MINUTE = 5;
        private const int VALIDITY_FOR_RESERVATION_HOUR = 24;
        private readonly IUserContextService _context;
        private readonly IBaseRepository<Order, Guid> _orderRepository;
        private readonly IBaseRepository<WarehouseSlot, Guid> _warehouseSlotRepository;
        private readonly IBaseRepository<SlotReservation, Guid> _slotReservationRepository;
        private readonly IBaseRepository<Contract, Guid> _contractRepository;

        public QuotationService(DeliverySytemContext db, IUserContextService context,
            IBaseRepository<Order, Guid> orderRepository,
            IBaseRepository<WarehouseSlot, Guid> warehouseSlotRepository,
            IBaseRepository<SlotReservation, Guid> slotReservationRepository,
            IBaseRepository<Contract, Guid> contractRepository)
        {
            _db = db;
            _context = context;
            _orderRepository = orderRepository;
            _warehouseSlotRepository = warehouseSlotRepository;
            _slotReservationRepository = slotReservationRepository;
            _contractRepository = contractRepository;
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
                ValidUntil = DateTime.UtcNow.AddMinutes(VALIDITY_MINUTE),
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
            using var transaction = await _db.Database.BeginTransactionAsync(ct);
            try
            {
                int SENT_VALID_HOUR = 48;
                var quotation = await _db.Quotations.Where(e => e.Id == vm.QuotationId).FirstOrDefaultAsync();
            
                quotation!.Status = StatusValue.Sent;
                quotation!.ValidUntil = DateTime.UtcNow.AddHours(SENT_VALID_HOUR);
                await CreateOrderAndSlotReservationsAsync(quotation, vm.SlotIds, ct, vm.From, vm.To);
                await _db.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync(ct);
                return false;
            }

           
            
            
            return true;
        }

        public async Task<bool> AcceptQuotationAsync(AcceptQuoteVm vm, CancellationToken ct)
        {
            using var transaction = await _db.Database.BeginTransactionAsync(ct);
            try
            {
                var quotation = await _db.Quotations.FindAsync(vm.QuotationId);
                if (quotation == null) return false;

                quotation.Status = StatusValue.Active; // Or whatever status means "accepted"

                await CreateOrderAndSlotReservationsAsync(quotation, vm.SlotIds, ct, vm.From, vm.To);

                await transaction.CommitAsync(ct);
                return true;
            }
            catch
            {
                await transaction.RollbackAsync(ct);
                return false;
            }
        }

        public async Task<bool> RequestRevisionAsync(RequestRevisionVm vm, CancellationToken ct)
        {
            using var transaction = await _db.Database.BeginTransactionAsync(ct);
            try
            {
                var quotation = await _db.Quotations.FindAsync(vm.QuotationId);
                if (quotation == null) return false;
                quotation.Status = StatusValue.Revised;

                await CreateOrderAndSlotReservationsAsync(quotation, vm.SlotIds, ct, vm.From, vm.To);
                await _db.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);
                return true;
            }
            catch
            {
                await transaction.RollbackAsync(ct);
                return false;
            }
        }

        public async Task<Quotation> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var quotation = await _db.Quotations.FindAsync(id, ct);
            if (quotation == null) return null;
            return quotation;
        }

        private async Task<(Order, List<SlotReservation>)> CreateOrderAndSlotReservationsAsync(
            Quotation quotation,
            List<Guid> slotIds,
            CancellationToken ct,
            DateTime from, DateTime to)
        {
            // Tạo order
            var order = new Order
            {
                Id = Guid.NewGuid(),
                QuotationId = quotation.Id,
                CustomerId = quotation.CustomerId,
                StoreId = quotation.StoreId ?? Guid.Empty,
                Status = StatusValue.Pending,
                TotalAmount = quotation.TotalAmount,
                Note = "Order auto-created from quotation action"
            };
            var newOrder = await _orderRepository.AddAsync(order);

            // Tạo slot reservations
            var slots = await _warehouseSlotRepository.FindAll(e => slotIds.Contains(e.Id)).ToListAsync(ct);
            var reservations = new List<SlotReservation>();
            foreach (var slot in slots)
            {
                reservations.Add(new SlotReservation
                {
                    Id = Guid.NewGuid(),
                    OrderId = newOrder.Id,
                    WarehouseSlotId = slot.Id,
                    ExpiresAt = DateTime.UtcNow.AddHours(VALIDITY_FOR_RESERVATION_HOUR),
                    Status = StatusValue.Reserved,
                    From = from,
                    To = to
                });
            }

            await _slotReservationRepository.AddRangeAsync(reservations);
            return (newOrder, reservations);
        }
    }
}