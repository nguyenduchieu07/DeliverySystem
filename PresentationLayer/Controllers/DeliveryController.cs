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

namespace PresentationLayer.Controllers
{
    [Authorize]
    public class DeliveryController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IDeliveryService _deliveryService;
        private readonly IOrderService _orderService;

        public DeliveryController(ICustomerService customerService, IDeliveryService deliveryService, IOrderService orderService)
        {
            _customerService = customerService;
            _deliveryService = deliveryService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
            {
                return Unauthorized("ID người dùng không hợp lệ hoặc bị thiếu.");
            }

            var customer = await _customerService.GetProfileAsync(userId);
            var addresses = await _customerService.GetAddressesAsync(userId) ?? new List<DataAccessLayer.Entities.Address>();
            var orders = await _customerService.GetOrdersAsync(userId) ?? new List<DataAccessLayer.Entities.Order>();

            var model = new ProfileViewModel
            {
                Customer = customer ?? new DataAccessLayer.Entities.Customer(),
                Addresses = addresses.Select(a => new AddressViewModel
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    StoreId = a.StoreId,
                    Label = a.Label,
                    AddressLine = a.AddressLine,
                    Ward = a.Ward,
                    District = a.District,
                    City = a.City,
                    Latitude = a.Latitude,
                    Longitude = a.Longitude,
                    IsDefault = a.IsDefault,
                    Active = a.Active
                }).ToList(),
                Orders = orders,
                NewAddress = new AddressViewModel()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWarehouseOrder([FromBody] CreateOrderViewModel viewModel)
        {
            Console.WriteLine("CreateWarehouseOrder called with viewModel: " + JsonSerializer.Serialize(viewModel));

            if (viewModel == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            // Validate Product Category
            if (viewModel.ProductCategories == null || !viewModel.ProductCategories.Any())
            {
                return BadRequest("Loại hàng hóa là bắt buộc.");
            }

            // Validate addresses
            if (viewModel.PickupAddress == null || viewModel.DropoffAddress == null)
            {
                return BadRequest("Địa chỉ lấy hàng và giao hàng là bắt buộc.");
            }

            // Validate pickup address
            if (string.IsNullOrWhiteSpace(viewModel.PickupAddress.AddressLine))
            {
                return BadRequest("Địa chỉ lấy hàng không được để trống.");
            }

            // Validate dropoff address  
            if (string.IsNullOrWhiteSpace(viewModel.DropoffAddress.AddressLine))
            {
                return BadRequest("Địa chỉ giao hàng không được để trống.");
            }

            Console.WriteLine($"Product Categories: {string.Join(", ", viewModel.ProductCategories)}");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
            {
                return Unauthorized("ID người dùng không hợp lệ hoặc bị thiếu.");
            }

            try
            {
                var dto = MapViewModelToDto(viewModel);

                // Tìm kho gần nhất
                var storeId = await _deliveryService.FindNearestStoreAsync(
                    dto.DropoffAddress.Latitude,
                    dto.DropoffAddress.Longitude
                );

                // Tạo order
                var order = await _deliveryService.CreateOrderFromDto(dto, userId, storeId);

                // Thông báo cho các kho lân cận
                var nearbyStoresCount = await _deliveryService.NotifyNearbyWarehousesAsync(order.Id);

                return Json(new
                {
                    success = true,
                    orderId = order.Id,
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
            double defaultRadius = 1000000.0;
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

        public IActionResult BookDelivery()
        {
            return RedirectToAction("Index");
        }
    }
}