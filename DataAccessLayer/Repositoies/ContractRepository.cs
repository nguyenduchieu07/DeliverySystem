using DataAccessLayer.Abstractions.IRepositories;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositoies;

public class ContractRepository : BaseRepository<Contract, Guid>, IContractRepository
{
    public ContractRepository(DeliverySytemContext context) : base(context)
    {
    }

    public async Task<List<Contract>> GetContractWithAllInfoAsync(Guid id) //id: quotationId
    {
        var contracts = await _context.Contracts
            .Include(c => c.Quotation)
            .ThenInclude(q => q.Customer)
            .Include(c => c.Warehouse)
            .Include(c => c.WarehouseSlot)
            .Include(c => c.Store)
            .Where(c => c.QuotationId == id).ToListAsync();
        return contracts;
    }
}