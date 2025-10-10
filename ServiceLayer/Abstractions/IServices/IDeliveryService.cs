using DataAccessLayer.Entities;
using ServiceLayer.DTOs;
using System;
using System.Threading.Tasks;

namespace ServiceLayer.Abstractions.IServices
{
    public interface IDeliveryService
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<int> NotifyNearbyWarehousesAsync(Guid orderId);
        Task<Guid> FindNearestStoreAsync(double latitude, double longitude);
        Task<Order> CreateOrderFromDto(CreateOrderDto dto, Guid userId, Guid storeId);
    }
}