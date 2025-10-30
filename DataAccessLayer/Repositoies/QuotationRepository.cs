using DataAccessLayer.Abstractions.IRepositories;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositoies;

public class QuotationRepository : BaseRepository<Quotation, Guid>, IQuotationRepository
{
    public QuotationRepository(DeliverySytemContext context) : base(context)
    {
    }

    public async Task<Quotation?> GetQuotationInfo(Guid quotationId)
    {
        return await _context.Quotations
            .Include(q => q.Customer)
            .Include(q => q.Store)
            .Include(q => q.Orders)
            .FirstOrDefaultAsync(q => q.Id == quotationId);
    }
}