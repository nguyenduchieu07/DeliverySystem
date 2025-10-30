using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;

namespace DataAccessLayer.Abstractions.IRepositories
{
    public interface IStoreRepository : IBaseRepository<Entities.Store, Guid>
    {
        Task<Store?> GetStoreWithDetailsAsync(Guid storeId);
    }
}
