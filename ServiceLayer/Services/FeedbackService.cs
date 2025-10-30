using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Abstractions.IRepositories;
using DataAccessLayer.Entities;
using ServiceLayer.Abstractions.IServices;

namespace ServiceLayer.Services
{
    internal class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        public FeedbackService(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }
        public async Task<List<Feedback>> GetAllFeedbacksByStoreId(Guid storeId)
        {
            return await _feedbackRepository.GetAllFeedbackByStoreIdAsync(storeId);
        }

        public async Task<Feedback> CreateFeedbackAsync(Feedback feedback)
        {
            return await _feedbackRepository.AddAsync(feedback);
        }
    }
}
