using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceLayer.Abstractions.IRepositories
{
    public interface IDeliveryRepository
    {
        Task<Order> AddOrderAsync(Order order);
        Task<Order> GetOrderByIdAsync(Guid orderId);
        Task<List<Store>> GetAllStoresAsync();
        Task<List<Store>> GetNearbyStoresAsync(double latitude, double longitude, double radiusKm);
    }
}