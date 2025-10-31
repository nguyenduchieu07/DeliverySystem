using DataAccessLayer.Entities;

namespace ServiceLayer.Abstractions.IServices;

public interface IContractService
{
    Task<List<Contract>> GenerateContractsAsync(Guid quotationId);
    Task<string> GenerateContractHtmlAsync(Guid contractId, bool forceGenerate = false);
    Task<List<Contract>> GetActiveQuotationContracts(Guid quotationId);
}