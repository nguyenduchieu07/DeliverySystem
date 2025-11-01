using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;

namespace ServiceLayer.Abstractions.IServices
{
    public interface IOrderService
    {
        Task<List<Order>> GetAllOrdersByStoreIdAsync(Guid storeId, StatusValue? status);
        Task<Order?> GetByIdAsync(Guid orderId);
        Task<Order?> GetOrderInfoByIdAsync(Guid orderId);
        bool UpdateOrder(Order order);
    }
}
