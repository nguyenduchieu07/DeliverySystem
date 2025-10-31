using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using ServiceLayer.Abstractions.IServices;
using ServiceLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Text.Json;
using DataAccessLayer.Enums;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace PresentationLayer.Controllers
{
    [Authorize(Roles = "Customer")]
    public class DeliveryController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IDeliveryService _deliveryService;
        private readonly IOrderService _orderService;
        private readonly IFeedbackService _feedbackService;

        public DeliveryController(ICustomerService customerService, IDeliveryService deliveryService, IOrderService orderService, IFeedbackService feedbackService)
        {
            _customerService = customerService;
            _deliveryService = deliveryService;
            _orderService = orderService;
            _feedbackService = feedbackService;
        }

        public async Task<IActionResult> Index()
        {
            // Customer info for display (read-only)
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userIdClaim) && Guid.TryParse(userIdClaim, out Guid userId))
            {
                var customer = await _customerService.GetProfileAsync(userId);
                if (customer != null)
                {
                    ViewBag.CustomerFullName = customer.FullName;
                    ViewBag.CustomerEmail = customer.Email;
                    ViewBag.CustomerPhone = customer.PhoneNumber;
                }
            }

            // Build BookingRequestVM for warehouse booking
            List<AddressOptionVM> addressOptions = new();
            DataAccessLayer.Entities.Address? defaultAddress = null;
            if (!string.IsNullOrEmpty(userIdClaim) && Guid.TryParse(userIdClaim, out userId))
            {
                var userAddresses = await _customerService.GetAddressesAsync(userId) ?? new List<DataAccessLayer.Entities.Address>();
                defaultAddress = userAddresses
                    .OrderByDescending(a => a.IsDefault)
                    .FirstOrDefault();

                addressOptions = userAddresses
                    .OrderByDescending(a => a.IsDefault)
                    .Select(a => new AddressOptionVM
                    {
                        Id = a.Id,
                        Label = string.IsNullOrWhiteSpace(a.Label) ? a.AddressLine : a.Label,
                        Full = a.AddressLine + ", " + (a.Ward ?? "") + ", " + (a.District ?? "") + ", " + (a.City ?? ""),
                        Latitude = a.Latitude,
                        Longitude = a.Longitude
                    })
                    .ToList();
            }

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

            // Pre-seed items with Cloudinary images
            vm.Items = new List<BookingItemVM>
            {
                new BookingItemVM
                {
                    Name = "Bàn học sinh",
                    Category = "Nội thất",
                    EstimatedWeightKg = 15,
                    Quantity = 15,
                    ImageUrl = "https://res.cloudinary.com/demo/image/upload/student_desk.jpg" // Thay bằng URL thật từ Cloudinary
                },
                new BookingItemVM
                {
                    Name = "Ghế học sinh",
                    Category = "Nội thất",
                    EstimatedWeightKg = 8,
                    Quantity = 15,
                    ImageUrl = "https://res.cloudinary.com/demo/image/upload/student_chair.jpg"
                },
                new BookingItemVM
                {
                    Name = "Bàn giáo viên",
                    Category = "Nội thất",
                    EstimatedWeightKg = 25,
                    Quantity = 1,
                    ImageUrl = "https://res.cloudinary.com/demo/image/upload/teacher_desk.jpg"
                },
                new BookingItemVM
                {
                    Name = "Ghế giáo viên",
                    Category = "Nội thất",
                    EstimatedWeightKg = 12,
                    Quantity = 1,
                    ImageUrl = "https://res.cloudinary.com/demo/image/upload/teacher_chair.jpg"
                },
                new BookingItemVM
                {
                    Name = "Tủ đựng tài liệu",
                    Category = "Nội thất",
                    EstimatedWeightKg = 45,
                    Quantity = 2,
                    ImageUrl = "https://res.cloudinary.com/demo/image/upload/file_cabinet.jpg"
                },
                new BookingItemVM
                {
                    Name = "Bảng trắng",
                    Category = "Thiết bị",
                    EstimatedWeightKg = 20,
                    Quantity = 1,
                    ImageUrl = "https://res.cloudinary.com/demo/image/upload/whiteboard.jpg"
                }
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWarehouseOrder([FromBody] CreateWarehouseOrderViewModel viewModel)
        {
            Console.WriteLine("CreateWarehouseOrder called with viewModel: " + JsonSerializer.Serialize(viewModel));

            if (viewModel == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            // Validate addresses
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

            // Validate dates
            if (viewModel.StorageEndDate <= viewModel.StorageStartDate)
            {
                return BadRequest("Ngày xuất kho phải sau ngày nhập kho.");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
            {
                return Unauthorized("ID người dùng không hợp lệ hoặc bị thiếu.");
            }

            try
            {
                // Tìm kho gần nhất dựa trên warehouse area
                var storeId = await _deliveryService.FindNearestStoreAsync(
                    viewModel.WarehouseArea.Latitude ?? 0,
                    viewModel.WarehouseArea.Longitude ?? 0
                );

                // Tạo order
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

                // Thêm special requirements vào note
                if (viewModel.SpecialRequirements != null && viewModel.SpecialRequirements.Any())
                {
                    order.Note += "\n\n📋 Yêu cầu đặc biệt:\n" + string.Join("\n", viewModel.SpecialRequirements.Select(r => "• " + r));
                }

                // Thêm items
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

                // Thông báo cho các kho lân cận
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

        private CreateOrderDto MapViewModelToDto(CreateOrderViewModel viewModel)
        {
            if (viewModel.PickupAddress == null || viewModel.DropoffAddress == null)
            {
                throw new ArgumentNullException("PickupAddress and DropoffAddress cannot be null.");
            }

            return new CreateOrderDto
            {
                PickupAddress = new AddressDto
                {
                    AddressLine = viewModel.PickupAddress.AddressLine,
                    Latitude = viewModel.PickupAddress.Latitude ?? 0,
                    Longitude = viewModel.PickupAddress.Longitude ?? 0,
                    RecipientName = viewModel.PickupAddress.RecipientName,
                    RecipientPhone = viewModel.PickupAddress.RecipientPhone,
                    Floor = !string.IsNullOrEmpty(viewModel.PickupAddress.Floor) &&
                            int.TryParse(viewModel.PickupAddress.Floor, out int floorNum)
                            ? (int?)floorNum
                            : null
                },
                DropoffAddress = new AddressDto
                {
                    AddressLine = viewModel.DropoffAddress.AddressLine,
                    Latitude = viewModel.DropoffAddress.Latitude ?? 0,
                    Longitude = viewModel.DropoffAddress.Longitude ?? 0
                },
                DistanceKm = viewModel.DistanceKm,
                EtaMinutes = viewModel.EtaMinutes,
                DeliveryDate = viewModel.DeliveryDate,
                PickupDate = viewModel.PickupDate,
                Note = viewModel.Note,

                // ✅ GIỮ: ProductCategories
                ProductCategories = viewModel.ProductCategories,
                EstimatedWeight = viewModel.EstimatedWeight,

                Items = viewModel.Items?.Select(i => new OrderItemDto
                {
                    Name = i.Name,
                    Quantity = i.Quantity
                }).ToList()
            };
        }

        public async Task<IActionResult> OrderAsync(string orderId)
        {
            double defaultRadius = 10.0;
            var vm = new DeliveryOrderViewModel();

            var canParse = Guid.TryParse(orderId, out Guid orderIdGuid);

            if (!canParse)
            {
                TempData["Error"] = "KHông tìm tìm thấy đơn hàng. Mã đơn hàng không hợp lệ";
                return View(vm);
            }
            var orderById = await _orderService.GetByIdAsync(orderIdGuid);

            if (orderById == null)
            {
                TempData["Error"] = "KHông tìm tìm thấy đơn hàng.";
                return View(vm);
            }

            vm.Order = orderById;

            var nearbyStores = await _deliveryService.GetNearbyStoresAsync(orderById.DropoffAddress?.Latitude ?? 0, orderById.DropoffAddress?.Longitude ?? 0, defaultRadius);

            vm.NearbyStores = nearbyStores;
            return View(vm);
        }

        public async Task<IActionResult> StoreFeedbackAsync(Guid storeId)
        {
            var feedbacks = await _feedbackService.GetAllFeedbacksByStoreId(storeId);
            return View(feedbacks);
        }

        public IActionResult BookDelivery()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetDefaultAddress(Guid addressId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
            {
                return Unauthorized("ID người dùng không hợp lệ hoặc bị thiếu.");
            }

            var addresses = await _db.Addresses.Where(a => a.UserId == userId && a.Active).ToListAsync();
            foreach (var a in addresses)
            {
                a.IsDefault = a.Id == addressId;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}