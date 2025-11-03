using DataAccessLayer.Entities;
using DataAccessLayer.Enums;

namespace ServiceLayer.Abstractions.IServices;

public interface IContractService
{
    Task<List<Contract>> GenerateContractsAsync(Guid quotationId);
    Task<Contract> GenerateContractAsync(Guid quotationId);
    Task<string> GenerateContractHtmlAsync(Guid contractId, bool forceGenerate = false);
    Task<List<Contract>> GetActiveQuotationContracts(Guid quotationId);

    Task<bool> ConfirmContract(Guid contractId, Guid orderId); //ContractStatus.Active
    Task<bool> CancleContract(Guid contractId, Guid orderId); //ContractStatus.Terminated

}