using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;

namespace ServiceLayer.Abstractions.IServices
{
    public interface IWarehouseService
    {
        Task<WarehouseSlot?> GetSlotByOrderIdAsync(Guid orderId);
    }
}
