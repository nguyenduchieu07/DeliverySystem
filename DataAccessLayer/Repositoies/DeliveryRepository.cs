using DataAccessLayer.Abstractions.IRepositories;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositoies;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceLayer.Abstractions.IRepositories;

namespace DataAccessLayer.Repositoies
{
    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly IBaseRepository<Order, Guid> _orderRepository;
        private readonly IBaseRepository<Store, Guid> _storeRepository;

        public DeliveryRepository(IBaseRepository<Order, Guid> orderRepository, IBaseRepository<Store, Guid> storeRepository)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _storeRepository = storeRepository ?? throw new ArgumentNullException(nameof(storeRepository));
        }

        public async Task<Order> AddOrderAsync(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }
            return await _orderRepository.AddAsync(order);
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            return await _orderRepository.GetByIdAsync(orderId);
        }

        public async Task<List<Store>> GetAllStoresAsync()
        {
            return await _storeRepository.GetAllAsync();
        }

        public async Task<List<Store>> GetNearbyStoresAsync(double latitude, double longitude, double radiusKm)
        {
            var stores = await _storeRepository.GetAllAsync();
            return stores.Where(s => CalculateDistance(latitude, longitude, s.Latitude ?? 0, s.Longitude ?? 0) <= radiusKm).ToList();
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