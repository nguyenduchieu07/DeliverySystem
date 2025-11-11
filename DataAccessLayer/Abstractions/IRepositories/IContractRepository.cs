using DataAccessLayer.Entities;

namespace DataAccessLayer.Abstractions.IRepositories;

public interface IContractRepository : IBaseRepository<Contract, Guid>
{
    public Task<List<Contract>> GetContractWithAllInfoAsync(Guid id);
}