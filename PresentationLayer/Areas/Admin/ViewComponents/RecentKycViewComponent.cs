using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace PresentationLayer.Areas.Admin.ViewComponents
{
    public class RecentKycViewComponent : ViewComponent
    {
        private readonly DeliverySytemContext _db;
        public RecentKycViewComponent(DeliverySytemContext db) => _db = db;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _db.Set<KycSubmission>()
                .Include(x => x.Store)
                .Where(x => x.Status == KycStatus.Pending)
                .OrderBy(x => x.SubmittedAt)
                .Take(5)
                .ToListAsync();
            return View(items);
        }
    }
}
