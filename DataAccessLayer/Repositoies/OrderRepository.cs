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
                 .ThenInclude( oi => oi.IncidentReports).ThenInclude(ir => ir.Actions).ThenInclude(a => a.Staff)
                 .Include(o => o.OrderWarehouseSlots).ThenInclude(ows => ows.WarehouseSlot).ThenInclude(ws => ws.Warehouse)
                 .Include(o => o.DropoffAddress)
                 .Include(o => o.PickupAddress)
                 .Include(o => o.Quotation)
                 .Include(o => o.Store);
            return await query.SingleOrDefaultAsync(x => x.Id.Equals(orderId));
        }

        public async Task<List<Order>> GetOrdersInfoByStoreIdAsync(Guid storeId, StatusValue? status = null)
        {
            var query = _context.Orders.AsNoTracking();
            if (status != null)
            {
                query = query.Where(x => x.Status == status);
            }

            query = query
              .Include(o => o.Customer)
              .Include(o => o.Store)
              .Include(o => o.PickupAddress)
              .Include(o => o.DropoffAddress)
              .Include(o => o.OrderItems)
                  .ThenInclude(oi => oi.Service)
              .Include(o => o.OrderItems)
                  .ThenInclude(oi => oi.IncidentReports)
                      .ThenInclude(ir => ir.Actions)
              .Include(o => o.Quotation)
              .Include(o => o.OrderWarehouseSlots)
                  .ThenInclude(ows => ows.WarehouseSlot);
            return await query.Where(x => x.StoreId.Equals(storeId)).ToListAsync();
        }

    }
}
