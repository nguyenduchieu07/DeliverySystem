using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Abstractions.IRepositories;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositoies
{
    internal class StoreRepository : BaseRepository<Store, Guid>, IStoreRepository
    {
        public StoreRepository(DeliverySytemContext context) : base(context)
        {
        }

        public async Task<Store?> GetStoreWithDetailsAsync(Guid storeId)
        {
            var store = await _context.Stores
                .AsNoTracking()
                .Include(s => s.Warehouses)
                .ThenInclude(w => w.Address)
                .FirstOrDefaultAsync(s => s.Id == storeId);

            return store;
        }
    }
}
