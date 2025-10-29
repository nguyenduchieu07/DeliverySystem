using DataAccessLayer.Abstractions.IRepositories;
using DataAccessLayer.Entities;

namespace DataAccessLayer.Repositoies;

public class ContractRepository : BaseRepository<Contract, Guid>, IContractRepository
{
    public ContractRepository(DeliverySytemContext context) : base(context)
    {
    }
}