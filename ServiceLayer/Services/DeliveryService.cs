using ServiceLayer.DTOs;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ServiceLayer.Abstractions.IServices;
using ServiceLayer.Abstractions.IRepositories;

namespace ServiceLayer.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly ILogger<DeliveryService> _logger;

        public DeliveryService(IDeliveryRepository deliveryRepository, ILogger<DeliveryService> logger)
        {
            _deliveryRepository = deliveryRepository ?? throw new ArgumentNullException(nameof(deliveryRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order), "Order cannot be null.");
            }

            try
            {
                // Lưu order vào database
                order.CreatedAt = DateTime.Now;
                order.UpdatedAt = DateTime.Now;
                order.Status = "Pending"; // Giả định trạng thái ban đầu
                var createdOrder = await _deliveryRepository.AddOrderAsync(order);
                _logger.LogInformation("Order {OrderId} created successfully.", createdOrder.Id);
                return createdOrder;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<int> NotifyNearbyWarehousesAsync(Guid orderId)
        {
            try
            {
                // Lấy thông tin order
                var order = await _deliveryRepository.GetOrderByIdAsync(orderId);
                if (order == null)
                {
                    throw new InvalidOperationException($"Order with ID {orderId} not found.");
                }
                // Giả lập logic tìm các kho lân cận (thay bằng logic thực tế)
                var nearbyStores = await _deliveryRepository.GetNearbyStoresAsync(order.DropoffAddress?.Latitude ?? 0, order.DropoffAddress?.Longitude ?? 0, 10); // 10km radius
                if (nearbyStores == null || !nearbyStores.Any())
                {
                    _logger.LogWarning("No nearby stores found for order {OrderId}.", orderId);
                    return 0;
                }
                // Giả lập thông báo cho các kho (thay bằng gọi API hoặc queue)
                int notifiedCount = nearbyStores.Count();
                _logger.LogInformation("Notified {Count} nearby stores for order {OrderId}.", notifiedCount, orderId);
                return notifiedCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error notifying nearby warehouses for order {OrderId}: {Message}", orderId, ex.Message);
                throw;
            }
        }

        public async Task<Guid> FindNearestStoreAsync(double latitude, double longitude)
        {
            try
            {
                // Giả lập logic tìm kho gần nhất (thay bằng thuật toán Haversine hoặc API)
                var stores = await _deliveryRepository.GetAllStoresAsync();
                if (stores == null || !stores.Any())
                {
                    _logger.LogWarning("No stores available to find nearest.");
                    throw new InvalidOperationException("No stores available.");
                }
                var nearestStore = stores
                    .OrderBy(s => CalculateDistance(latitude, longitude, s.Latitude ?? 0, s.Longitude ?? 0))
                    .First();
                _logger.LogInformation("Nearest store found: {StoreId} for location ({Latitude}, {Longitude}).", nearestStore.Id, latitude, longitude);
                return nearestStore.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error finding nearest store for location ({Latitude}, {Longitude}): {Message}", latitude, longitude, ex.Message);
                throw;
            }
        }

        public async Task<Order> CreateOrderFromDto(CreateOrderDto dto, Guid userId, Guid storeId)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto), "DTO cannot be null.");
            }

            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = userId,
                StoreId = storeId,
                PickupAddress = new Address
                {
                    Id = Guid.NewGuid(),
                    AddressLine = dto.PickupAddress.AddressLine,
                    Latitude = dto.PickupAddress.Latitude,
                    Longitude = dto.PickupAddress.Longitude,
                    City = "Hà Nội"
                },
                DropoffAddress = new Address
                {
                    Id = Guid.NewGuid(),
                    AddressLine = dto.DropoffAddress.AddressLine,
                    Latitude = dto.DropoffAddress.Latitude,
                    Longitude = dto.DropoffAddress.Longitude,
                    City = "Hà Nội"
                },
                DistanceKm = dto.DistanceKm,
                EtaMinutes = dto.EtaMinutes,
                DeliveryDate = dto.DeliveryDate,
                PickupDate = dto.PickupDate,
                Note = dto.Note,
                Status = "Pending",
                TotalAmount = 0m,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                OrderItems = dto.Items?.Select(i => new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ServiceId = Guid.NewGuid(),
                    Quantity = i.Quantity,
                    UnitPrice = 0m,
                    Subtotal = 0m
                }).ToList() ?? new List<OrderItem>()
            };

            return await CreateOrderAsync(order);
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Bán kính Trái Đất (km)
            var dLat = ToRadian(lat2 - lat1);
            var dLon = ToRadian(lon2 - lon1);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadian(lat1)) * Math.Cos(ToRadian(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private double ToRadian(double degree)
        {
            return degree * Math.PI / 180;
        }
    }
}