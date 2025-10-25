using DataAccessLayer.Abstractions.IRepositories;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class FeedbackController : Controller
    {
        private DeliverySytemContext _db;
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackController(DeliverySytemContext db, IFeedbackRepository feedbackRepository)
        {
            _db = db;
            _feedbackRepository = feedbackRepository;
        }
        public async Task<IActionResult> Index()
        {
            //return store to get each one reviews
            var stores = await _db.Stores.AsNoTracking().ToListAsync();
            return View(stores);

        }

        public async Task<IActionResult> GetFeedbacksByStoreAsync(Guid storeId)
        {
            var feedbacks = await _feedbackRepository.GetAllFeedbackByStoreIdAsync(storeId);

            return View(feedbacks);
        }

    }
}
