using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Areas.Admin.Models;
using System;

namespace PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly DeliverySytemContext _db;
        public HomeController(DeliverySytemContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var today = DateTime.Now.Date;
            var vm = new DashboardVM
            {
                PendingKyc = await _db.Set<KycSubmission>().CountAsync(x => x.Status == KycStatus.Pending),
                ActiveStores = await _db.Stores.CountAsync(s => s.Status == StatusValue.Active),
                OrdersToday = await _db.Orders.CountAsync(o => o.CreatedAt >= today && o.CreatedAt < today.AddDays(1)),
                LowRatingCount = await _db.Feedbacks.CountAsync(f => f.Rating <= 2)
            };
            return View(vm);
        }
    }
}
