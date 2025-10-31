using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataAccessLayer.Abstractions.IRepositories;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using ServiceLayer.Abstractions.IServices;

namespace ServiceLayer.Services
{
    public class OrderService : IOrderService
    {
        private readonly DeliverySytemContext _context;
        private readonly IBaseRepository<Order, Guid> _orderRepository;
        private readonly IQuotationRepository _quotationRepository;
        public OrderService(DeliverySytemContext context, IBaseRepository<Order, Guid>  orderRepository, IQuotationRepository quotationRepository)
        {
            _context = context;
            _orderRepository = orderRepository;
            _quotationRepository = quotationRepository;
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
