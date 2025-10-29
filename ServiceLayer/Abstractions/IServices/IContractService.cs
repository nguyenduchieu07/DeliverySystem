using DataAccessLayer.Entities;

namespace ServiceLayer.Abstractions.IServices;

public interface IContractService
{
    Task<List<Contract>> GenerateContractsync(Guid quotationId);
}