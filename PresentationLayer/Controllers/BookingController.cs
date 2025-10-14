using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers
{
    public class BookingController : Controller
    {
        private readonly DeliverySytemContext _db;
        public BookingController(DeliverySytemContext deliverySytemContext)
        {
            _db = deliverySytemContext;
        }
        public async Task<IActionResult> Index()
        {
            var userId = "1";


            var vm = new BookingRequestVM
            {
                PickupDate = DateTime.UtcNow.Date.AddDays(1),
                PickupTime = "09:00",
                AddressOptions = await _db.Addresses
            //.Where(a => a.UserId == userId && a.Active)
            .OrderByDescending(a => a.IsDefault)
            .Select(a => new AddressOptionVM
            {
                Id = a.Id,
                Label = string.IsNullOrWhiteSpace(a.Label) ? a.AddressLine : a.Label,
                Full = a.AddressLine + ", " + (a.Ward ?? "") + ", " + (a.District ?? "") + ", " + (a.City ?? "")
            })
            .ToListAsync(),
                Items = { new BookingItemVM() }
            };


            ViewBag.Services = await _db.Services
            .Where(s => s.IsActive)
            .OrderBy(s => s.Name)
            .Select(s => new { s.Id, s.Name, s.Unit, s.BasePrice })
            .ToListAsync();


            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userId = "1";


            var vm = new BookingRequestVM
            {
                PickupDate = DateTime.UtcNow.Date.AddDays(1),
                PickupTime = "09:00",
                AddressOptions = await _db.Addresses
            //.Where(a => a.UserId == userId && a.Active)
            .OrderByDescending(a => a.IsDefault)
            .Select(a => new AddressOptionVM
            {
                Id = a.Id,
                Label = string.IsNullOrWhiteSpace(a.Label) ? a.AddressLine : a.Label,
                Full = a.AddressLine + ", " + (a.Ward ?? "") + ", " + (a.District ?? "") + ", " + (a.City ?? "")
            })
            .ToListAsync(),
                Items = { new BookingItemVM() }
            };


            ViewBag.Services = await _db.Services
            .Where(s => s.IsActive)
            .OrderBy(s => s.Name)
            .Select(s => new { s.Id, s.Name, s.Unit, s.BasePrice })
            .ToListAsync();


            return View(vm);
        
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookingRequestVM vm)
        {
            var userId = new Guid();

            if (!ModelState.IsValid)
            {
                await RehydrateServices();
                await RehydrateAddresses(userId, vm);
                return View(vm);
            }

            // Resolve Customer
            var customer = await _db.Users
                .Include(u => u.Customer)
                .Where(u => u.Id == userId)
                .Select(u => u.Customer)
                .FirstOrDefaultAsync();
            if (customer == null)
            {
                ModelState.AddModelError("", "Tài khoản chưa có hồ sơ khách hàng.");
                await RehydrateServices();
                await RehydrateAddresses(userId, vm);
                return View(vm);
            }

            // For MVP, pick the first store that offers the first selected service (or a fixed store if your flow enforces choosing a store).
            var firstServiceId = vm.Items.First().ServiceId;
            var storeId = await _db.Services.Where(s => s.Id == firstServiceId).Select(s => s.StoreId).FirstAsync();

            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = customer.Id,
                StoreId = storeId,
                QuotationId = null,
                PickupAddressId = vm.PickupAddressId,
                DropoffAddressId = vm.DropoffAddressId,
                DistanceKm = null, // can be filled later via routing service
                EtaMinutes = null,
                Status = DataAccessLayer.Enums.StatusValue.Pending, // created → pending review/quotation
                TotalAmount = 0m, // compute later or as estimate
                Note = vm.Note,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            foreach (var it in vm.Items.Where(i => i.ServiceId != Guid.Empty && i.Quantity > 0))
            {
                var svc = await _db.Services.FindAsync(it.ServiceId);
                if (svc == null) continue;
                var price = await _db.ServicePrices
                    .Where(p => p.ServiceId == svc.Id && p.ValidFrom <= DateTime.UtcNow && p.ValidTo >= DateTime.UtcNow)
                    .OrderByDescending(p => p.ValidFrom)
                    .Select(p => p.Price)
                    .FirstOrDefaultAsync();
                var unitPrice = price == 0 ? svc.BasePrice : price;

                order.OrderItems.Add(new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ServiceId = svc.Id,
                    Quantity = it.Quantity,
                    UnitPrice = unitPrice,
                    Subtotal = unitPrice * it.Quantity
                });
            }

            order.TotalAmount = order.OrderItems.Sum(oi => oi.Subtotal);

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            TempData["Toast"] = "Đã tạo yêu cầu gửi đồ. Cửa hàng sẽ phản hồi báo giá sớm.";
            return RedirectToAction(nameof(Success), new { id = order.Id });
        }

        public async Task<IActionResult> Success(Guid id)
        {
            var order = await _db.Orders
                .Include(o => o.PickupAddress)
                .Include(o => o.DropoffAddress)
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Service)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound();
            return View(order);
        }

        private async Task RehydrateServices()
        {
            ViewBag.Services = await _db.Services
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .Select(s => new { s.Id, s.Name, s.Unit, s.BasePrice })
                .ToListAsync();
        }

        private async Task RehydrateAddresses(Guid userId, BookingRequestVM vm)
        {
            vm.AddressOptions = await _db.Addresses
                .Where(a => a.UserId == userId && a.Active)
                .OrderByDescending(a => a.IsDefault)
                .Select(a => new AddressOptionVM
                {
                    Id = a.Id,
                    Label = string.IsNullOrWhiteSpace(a.Label) ? a.AddressLine : a.Label,
                    Full = a.AddressLine + ", " + (a.Ward ?? "") + ", " + (a.District ?? "") + ", " + (a.City ?? "")
                })
                .ToListAsync();
        }
    }
}
