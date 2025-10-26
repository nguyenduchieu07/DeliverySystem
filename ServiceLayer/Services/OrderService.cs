using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataAccessLayer.Abstractions.IRepositories;
using DataAccessLayer.Entities;
using ServiceLayer.Abstractions.IServices;

namespace ServiceLayer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBaseRepository<Order, Guid> _orderRepository;

        public OrderService(IBaseRepository<Order, Guid>  orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<Order?> GetByIdAsync(Guid orderId)
        {
            return await _orderRepository.FindSingleAsync(x => x.Id.Equals(orderId), cancellationToken: default, includeProperties: [o => o.PickupAddress,
                o => o.DropoffAddress,
                o => o.Store ]
            );
        }
    }
}
