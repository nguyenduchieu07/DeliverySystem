using DataAccessLayer.Entities;

namespace DataAccessLayer.Abstractions.IRepositories;

public interface IContractRepository : IBaseRepository<Contract, Guid>
{
    public Task<Contract> GetContractWithAllInfoAsync(Guid id);
}