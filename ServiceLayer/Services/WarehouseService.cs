using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Abstractions.IRepositories;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Abstractions.IServices;

namespace ServiceLayer.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IBaseRepository<WarehouseSlot, Guid> _warehouseSlotRepository;
        public WarehouseService(IBaseRepository<WarehouseSlot, Guid> warehouseSlotRepository)
        {
            _warehouseSlotRepository = warehouseSlotRepository;
        }

        public async Task<WarehouseSlot?> GetSlotByOrderIdAsync(Guid orderId)
        {
            var today = DateTime.UtcNow;

            return await _warehouseSlotRepository.FindAll(x => x.CurrentOrderId == orderId && x.LeaseStart <= today && x.LeaseEnd >= today)
                .OrderByDescending(s => s.CreatedAt)
                .FirstOrDefaultAsync();
        }
    }
}
