using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Http;
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
            // Điều hướng đến Create để đặt booking
            return await Create();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var addressOptions = await _db.Addresses
                //.Where(a => a.UserId == userId && a.Active)
                .OrderByDescending(a => a.IsDefault)
                .Select(a => new AddressOptionVM
                {
                    Id = a.Id,
                    Label = string.IsNullOrWhiteSpace(a.Label) ? a.AddressLine : a.Label,
                    Full = a.AddressLine + ", " + (a.Ward ?? "") + ", " + (a.District ?? "") + ", " + (a.City ?? ""),
                    Latitude = a.Latitude,
                    Longitude = a.Longitude
                })
                .ToListAsync();

            var defaultAddress = await _db.Addresses
                //.Where(a => a.UserId == userId && a.Active)
                .OrderByDescending(a => a.IsDefault)
                .FirstOrDefaultAsync();

            var vm = new BookingRequestVM
            {
                StorageStartDate = DateTime.UtcNow.Date.AddDays(1),
                StorageEndDate = DateTime.UtcNow.Date.AddDays(30),
                AddressOptions = addressOptions,
                DropoffAddressId = defaultAddress?.Id,
                DropoffLatitude = defaultAddress?.Latitude,
                DropoffLongitude = defaultAddress?.Longitude,
                DropoffAddressText = defaultAddress == null ? null : ($"{defaultAddress.AddressLine}, {defaultAddress.Ward}, {defaultAddress.District}, {defaultAddress.City}")?.Replace("  ", " ")
            };

            vm.Items = new List<BookingItemVM>
            {
                new BookingItemVM { Name = "Sofa da 3 chỗ", Category = "Đồ gỗ", EstimatedWeightKg = 60, Quantity = 1 },
                new BookingItemVM { Name = "Tủ lạnh 300L", Category = "Điện tử", EstimatedWeightKg = 50, Quantity = 1 },
                new BookingItemVM { Name = "Thùng sách", Category = "Giấy tờ", EstimatedWeightKg = 10, Quantity = 10 }
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingRequestVM vm, List<IFormFile>? Images)
        {
            if (!ModelState.IsValid)
            {
                await RehydrateAddresses(Guid.Empty, vm);
                return View(vm);
            }

            // TODO: lưu RFQ, upload ảnh, và gửi tới các kho phù hợp
            var fakeOrderId = Guid.NewGuid();
            return RedirectToAction("Success", new { id = fakeOrderId });
        }

        [HttpGet]
        public async Task<IActionResult> NearbyWarehouses(double lat, double lng, int take = 10)
        {
            // Ưu tiên kho có toạ độ rõ ràng
            var warehouses = await _db.Warehouses
                .Include(w => w.Address)
                .Include(w => w.Store)
                .Where(w => w.Address != null && w.Address.Latitude != null && w.Address.Longitude != null)
                .Select(w => new
                {
                    w.Id,
                    w.Name,
                    StoreName = w.Store.StoreName,
                    Latitude = w.Address!.Latitude!.Value,
                    Longitude = w.Address!.Longitude!.Value,
                    AddressLine = w.Address!.AddressLine,
                    Ward = w.Address!.Ward,
                    District = w.Address!.District,
                    City = w.Address!.City
                })
                .ToListAsync();

            // Tính khoảng cách cơ bản Haversine
            static double ToRad(double d) => d * Math.PI / 180.0;
            var results = warehouses
                .Select(w =>
                {
                    var R = 6371.0; // km
                    var dLat = ToRad(w.Latitude - lat);
                    var dLng = ToRad(w.Longitude - lng);
                    var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(ToRad(lat)) * Math.Cos(ToRad(w.Latitude)) * Math.Sin(dLng / 2) * Math.Sin(dLng / 2);
                    var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                    var dist = R * c;
                    return new
                    {
                        w.Id,
                        w.Name,
                        w.StoreName,
                        w.Latitude,
                        w.Longitude,
                        distanceKm = Math.Round(dist, 2),
                        full = string.Join(", ", new[] { w.AddressLine, w.Ward, w.District, w.City }.Where(s => !string.IsNullOrWhiteSpace(s)))
                    };
                })
                .OrderBy(x => x.distanceKm)
                .Take(take)
                .ToList();

            return Json(results);
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

        private async Task RehydrateServices() { await Task.CompletedTask; }

        private async Task RehydrateAddresses(Guid userId, BookingRequestVM vm)
        {
            vm.AddressOptions = await _db.Addresses
                //.Where(a => a.UserId == userId && a.Active)
                .OrderByDescending(a => a.IsDefault)
                .Select(a => new AddressOptionVM
                {
                    Id = a.Id,
                    Label = string.IsNullOrWhiteSpace(a.Label) ? a.AddressLine : a.Label,
                    Full = a.AddressLine + ", " + (a.Ward ?? "") + ", " + (a.District ?? "") + ", " + (a.City ?? ""),
                    Latitude = a.Latitude,
                    Longitude = a.Longitude
                })
                .ToListAsync();
        }
    }
}
