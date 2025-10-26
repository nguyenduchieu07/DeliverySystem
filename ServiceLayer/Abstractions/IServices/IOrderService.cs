using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;

namespace ServiceLayer.Abstractions.IServices
{
    public interface IOrderService
    {
        Task<Order?> GetByIdAsync(Guid orderId);
    }
}
