using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;

namespace DataAccessLayer.Abstractions.IRepositories
{
    public interface IFeedbackRepository : IBaseRepository<Feedback, Guid>
    {
        public Task<List<Feedback>> GetAllFeedbackByStoreIdAsync(Guid storeId);
    }
}
