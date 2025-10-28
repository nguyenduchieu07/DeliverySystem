
using DataAccessLayer.Abstractions.IRepositories;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositoies
{
    public class KycRepository : BaseRepository<KycSubmission,Guid>, IKycRepository
    {

        public KycRepository(DeliverySytemContext db) : base(db)
        {

        }
        public async Task<List<KycSubmission>> GetAllAsync(string storeName, KycStatus? status)
        {
            var qry = _context.KycSubmissions.AsNoTracking();
                         
            if(status != null)
                qry = qry.Where(x => x.Status == status);
            if (!string.IsNullOrWhiteSpace(storeName))
                qry = qry.Where(x => x.Store.StoreName.Contains(storeName));

            qry = qry.Include(x => x.Store).Include(x => x.Documents);
            var list = await qry.OrderByDescending(x => x.SubmittedAt).Take(200).ToListAsync();
            return list;
        }
        
        public async Task<KycSubmission> GetKycSubmissionByStoreId(Guid storeId)
        {
            var qry = _context.KycSubmissions.AsNoTracking();
            return await qry.Include(k => k.Documents).SingleOrDefaultAsync(x => x.StoreId.Equals(storeId));
        }
    }
}
