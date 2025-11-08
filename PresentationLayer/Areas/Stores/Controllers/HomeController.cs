using DataAccessLayer.Constants;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Abstractions.IServices;
using System.Security.Claims;
using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Identity;
using PresentationLayer.Areas.Stores.Models;

namespace PresentationLayer.Areas.Stores.Controllers
{
    [Area("Stores")]
    [Authorize(Roles = $"{UserRoles.STORE}, {UserRoles.STORESTAFF}")]
    public class HomeController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly DeliverySytemContext _db;
        private readonly UserManager<User> _userManager; 
        public HomeController(IDashboardService dashboardService, DeliverySytemContext db, UserManager<User> userManager)
        {
            _dashboardService = dashboardService;
            _db = db;
            _userManager = userManager;
        }
        
        
        
        public async Task<IActionResult> Index(DateTime? from = null, DateTime? to = null)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var id = Guid.Parse(userId!);

            var user = await _userManager.FindByIdAsync(userId);

            var isStoreRole = await _userManager.IsInRoleAsync(user, UserRoles.STORE);
            var isStoreStaffRole = await _userManager.IsInRoleAsync(user, UserRoles.STORESTAFF);

            Store? store = null;
            if (isStoreRole)
            {
                store = await _db.Stores.Where(e => e.OwnerUserId == id).FirstOrDefaultAsync();
            }
            else if(isStoreStaffRole){
                var staffStore = await _db.StoreStaffs.FirstAsync(x => x.UserId == id);
                store = await _db.Stores.Where(e => e.Id == staffStore.StoreId).FirstOrDefaultAsync();
            }else
            {   
                return RedirectToAction("Login", "Account");
            }


            if (store.Status == DataAccessLayer.Enums.StatusValue.Pending)
            {
                return BadRequest("Store is pending to approved by admin");
            }
            var reports =await _dashboardService.GetDashboard(store.Id);
            if(reports == null)
            {
                reports = new Models.DashboardDto();
            }
            
            var startDate = from ?? DateTime.Now.AddDays(-6); // default 7 ngày
            var endDate = to ?? DateTime.Now;
            
            // Lấy payments completed
            var payments = await _db.Orders
                .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate)
                .SelectMany(o => o.Payments
                    .Where(p => p.Status == StatusValue.Completed)
                    .Select(p => new { o.CreatedAt, p.Amount }))
                .ToListAsync();

            List<string> labels;
            List<decimal> data;

           
                var daily = payments
                    .GroupBy(x => x.CreatedAt.Date)
                    .Select(g => new
                    {
                        Date = g.Key,
                        Total = g.Sum(x => x.Amount)
                    })
                    .OrderBy(x => x.Date)
                    .ToList();

                labels = daily.Select(x => x.Date.ToString("dd/MM")).ToList();
                data = daily.Select(x => x.Total).ToList();
            
            var vm = new DashboardViewModel
            {
                DashboardDto = reports,
                RevenueLabels = labels,
                RevenueData = data
            };
            
            return View(vm);
        }
    }
}
