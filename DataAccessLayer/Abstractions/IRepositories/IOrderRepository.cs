using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;

namespace DataAccessLayer.Abstractions.IRepositories
{
    public interface IOrderRepository : IBaseRepository<Order, Guid>
    {
        Task<Order?> GetOrderInfoByIdAsync(Guid orderId);
        Task<List<Order>> GetOrdersInfoByStoreIdAsync(Guid orderId, StatusValue? status = null);
    }
}
