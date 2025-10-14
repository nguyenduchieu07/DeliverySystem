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

namespace PresentationLayer.Controllers
{
    [Authorize]
    public class DeliveryController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IDeliveryService _deliveryService;

        public DeliveryController(ICustomerService customerService, IDeliveryService deliveryService)
        {
            _customerService = customerService;
            _deliveryService = deliveryService;
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
            Console.WriteLine("CreateWarehouseOrder called with viewModel: " + System.Text.Json.JsonSerializer.Serialize(viewModel));
            if (viewModel == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
            {
                return Unauthorized("ID người dùng không hợp lệ hoặc bị thiếu.");
            }

            // Ánh xạ từ ViewModel sang DTO
            var dto = MapViewModelToDto(viewModel);
            // Giả định chọn kho gần nhất dựa trên dropoffAddress
            var storeId = await _deliveryService.FindNearestStoreAsync(dto.DropoffAddress.Latitude, dto.DropoffAddress.Longitude);
            // Gọi service để xử lý
            var order = await _deliveryService.CreateOrderFromDto(dto, userId, storeId);

            try
            {
                await _deliveryService.CreateOrderAsync(order);
                var nearbyStoresCount = await _deliveryService.NotifyNearbyWarehousesAsync(order.Id);
                return Json(new { success = true, orderId = order.Id, nearbyStoresCount = nearbyStoresCount, message = "Đơn hàng đã được tạo thành công!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in CreateWarehouseOrder: " + ex.Message);
                return Json(new { success = false, message = $"Lỗi khi tạo đơn hàng: {ex.Message}" });
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
                    Latitude = viewModel.PickupAddress.Latitude ?? 0, // Sử dụng giá trị mặc định nếu null
                    Longitude = viewModel.PickupAddress.Longitude ?? 0
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
                Items = viewModel.Items?.Select(i => new OrderItemDto
                {
                    Name = i.Name,
                    Quantity = i.Quantity
                }).ToList()
            };
        }

        public IActionResult Orders()
        {
            return View();
        }

        public IActionResult BookDelivery()
        {
            return RedirectToAction("Index");
        }
    }
}