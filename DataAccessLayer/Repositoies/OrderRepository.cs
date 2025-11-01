using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Abstractions.IRepositories;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositoies
{
    public class OrderRepository : BaseRepository<Order, Guid>, IOrderRepository
    {
        public OrderRepository(DeliverySytemContext context) : base(context)
        {
        }

        public async Task<Order?> GetOrderInfoByIdAsync(Guid orderId)
        {
            var query = _context.Orders.AsNoTracking();
             query = query
                 .Include(o => o.Customer)
                 .Include(o => o.OrderItems)
                 .Include(o => o.OrderWarehouseSlots).ThenInclude(ows => ows.WarehouseSlot).ThenInclude(ws => ws.Warehouse)
                 .Include(o => o.DropoffAddress)
                 .Include(o => o.PickupAddress)
                 .Include(o => o.Store);
            return await query.SingleOrDefaultAsync(x => x.Id.Equals(orderId));
        }

        public async Task<List<Order>> GetOrdersInfoByStoreIdAsync(Guid orderId, StatusValue? status = null)
        {
            var query = _context.Orders.AsNoTracking();
            if (status != null)
            {
                query = query.Where(x => x.Status == status);
            }
            
            query = query
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .Include(o => o.OrderWarehouseSlots).ThenInclude(ows => ows.WarehouseSlot)
                .Include(o => o.DropoffAddress)
                .Include(o => o.PickupAddress)
                .Include(o => o.Store);
            return await query.Where(x => x.StoreId.Equals(orderId)).ToListAsync();
        }
    }
}
