using DataAccessLayer.Entities;

namespace DataAccessLayer.Abstractions.IRepositories;

public interface IQuotationRepository : IBaseRepository<Quotation, Guid>
{
    Task<Quotation?> GetQuotationInfo(Guid quotationId);
}