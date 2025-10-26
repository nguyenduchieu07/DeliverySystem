using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using DataAccessLayer.Repositoies;

namespace DataAccessLayer.Abstractions.IRepositories
{
    public interface IKycRepository : IBaseRepository<KycSubmission, Guid>
    {
        public Task<List<KycSubmission>> GetAllAsync(string storeName, KycStatus? status);

    }
}
