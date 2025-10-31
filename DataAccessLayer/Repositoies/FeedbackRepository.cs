using DataAccessLayer.Abstractions.IRepositories;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositoies
{
    public class FeedbackRepository : BaseRepository<Feedback, Guid>, IFeedbackRepository
    {
        public FeedbackRepository(DeliverySytemContext context) : base(context)
        {
        }

        public async Task<List<Feedback>> GetAllFeedbackByStoreIdAsync(Guid storeId)
        {
            var query = _context.Feedbacks.AsNoTracking()
                .Where(f => f.ToStoreId.Equals(storeId))
                .Include(f => f.ToStore).Include(f => f.FromUser).ThenInclude(u => u.Customer);

            return await query.ToListAsync();

        }
    }
}
