using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Models;
using ServiceLayer.Abstractions.IServices;
using DataAccessLayer.Enums;

namespace PresentationLayer.Controllers
{

    public class BookingController : Controller
    {
        private readonly DeliverySytemContext _db;
        private readonly ICustomerService _customerService;
        private readonly IDeliveryService _deliveryService;
        private readonly IOrderService _orderService;
        public BookingController(DeliverySytemContext deliverySytemContext, ICustomerService customerService, IDeliveryService deliveryService, IOrderService orderService)
        {
            _db = deliverySytemContext;
            _customerService = customerService;
            _deliveryService = deliveryService;
            _orderService = orderService;
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

            // Prefill customer info from profile if available
            var userIdClaim = HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userIdClaim) && Guid.TryParse(userIdClaim, out Guid userId))
            {
                var customer = await _customerService.GetProfileAsync(userId);
                if (customer != null)
                {
                    vm.CustomerFullName = customer.FullName;
                    vm.CustomerEmail = customer.Email;
                    vm.CustomerPhone = customer.PhoneNumber;
                }
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWarehouseOrder([FromBody] CreateWarehouseOrderViewModel viewModel)
        {
            if (viewModel == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            if (viewModel.PickupAddress == null || viewModel.WarehouseArea == null)
            {
                return BadRequest("Địa chỉ nhận hàng và khu vực tìm kho là bắt buộc.");
            }

            if (string.IsNullOrWhiteSpace(viewModel.PickupAddress.AddressLine))
            {
                return BadRequest("Địa chỉ nhận hàng không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(viewModel.WarehouseArea.AddressLine))
            {
                return BadRequest("Khu vực tìm kho không được để trống.");
            }

            if (viewModel.StorageEndDate <= viewModel.StorageStartDate)
            {
                return BadRequest("Ngày xuất kho phải sau ngày nhập kho.");
            }

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
            {
                return Unauthorized("ID người dùng không hợp lệ hoặc bị thiếu.");
            }

            try
            {
                var storeId = await _deliveryService.FindNearestStoreAsync(
                    viewModel.WarehouseArea.Latitude ?? 0,
                    viewModel.WarehouseArea.Longitude ?? 0
                );

                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    CustomerId = userId,
                    StoreId = storeId,
                    PickupAddress = new Address
                    {
                        Id = Guid.NewGuid(),
                        AddressLine = viewModel.PickupAddress.AddressLine,
                        Latitude = viewModel.PickupAddress.Latitude,
                        Longitude = viewModel.PickupAddress.Longitude,
                        City = "Hà Nội"
                    },
                    DropoffAddress = new Address
                    {
                        Id = Guid.NewGuid(),
                        AddressLine = viewModel.WarehouseArea.AddressLine,
                        Latitude = viewModel.WarehouseArea.Latitude,
                        Longitude = viewModel.WarehouseArea.Longitude,
                        City = "Hà Nội"
                    },
                    DeliveryDate = viewModel.StorageStartDate,
                    PickupDate = viewModel.StorageEndDate,
                    Note = viewModel.Note ?? string.Empty,
                    Status = StatusValue.Pending,
                    TotalAmount = 0m,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    OrderItems = new List<OrderItem>()
                };

                if (viewModel.SpecialRequirements != null && viewModel.SpecialRequirements.Any())
                {
                    order.Note += "\n\n📋 Yêu cầu đặc biệt:\n" + string.Join("\n", viewModel.SpecialRequirements.Select(r => "• " + r));
                }

                if (viewModel.Items != null && viewModel.Items.Any())
                {
                    foreach (var itemVm in viewModel.Items)
                    {
                        if (string.IsNullOrWhiteSpace(itemVm.Name) || itemVm.Quantity <= 0)
                        {
                            continue;
                        }

                        order.OrderItems.Add(new OrderItem
                        {
                            Id = Guid.NewGuid(),
                            OrderId = order.Id,
                            ItemName = itemVm.Name.Trim(),
                            Description = itemVm.Category,
                            Quantity = itemVm.Quantity,
                            WeightKg = itemVm.EstimatedWeightKg,
                            UnitPrice = 0m,
                            Subtotal = 0m
                        });
                    }
                }

                var createdOrder = await _deliveryService.CreateOrderAsync(order);
                var nearbyStoresCount = await _deliveryService.NotifyNearbyWarehousesAsync(createdOrder.Id);

                return Json(new
                {
                    success = true,
                    orderId = createdOrder.Id,
                    nearbyStoresCount = nearbyStoresCount,
                    message = "Đơn hàng đã được tạo thành công!"
                });
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("No stores available: " + ex.Message);
                return Json(new { success = false, message = "Không tìm thấy kho hàng khả dụng." });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in CreateWarehouseOrder: " + ex.Message);
                Console.WriteLine("Inner exception: " + ex.InnerException?.Message);
                return Json(new
                {
                    success = false,
                    message = $"Lỗi khi tạo đơn hàng: {ex.Message}",
                    detail = ex.InnerException?.Message
                });
            }
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
      

        [HttpGet]
        public async Task<IActionResult> Success(Guid? Id)
        {
            // Support both 'Id' (from query string) and 'id' (from route)
            var orderId = Id ?? (Guid.TryParse(Request.Query["Id"].FirstOrDefault(), out var parsedId) ? parsedId : Guid.Empty);
            
            if (orderId == Guid.Empty)
            {
                return BadRequest("Mã đơn hàng không hợp lệ.");
            }

            var order = await _db.Orders
                .Include(o => o.Customer)
                .Include(o => o.PickupAddress)
                .Include(o => o.DropoffAddress)
                .Include(o => o.OrderItems)
                .Include(o => o.Store)
                .FirstOrDefaultAsync(o => o.Id == orderId);
            
            if (order == null)
            {
                return NotFound("Không tìm thấy đơn hàng với mã đã cung cấp.");
            }
            
            ViewData["Title"] = "Đơn hàng đã được tạo thành công";
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
