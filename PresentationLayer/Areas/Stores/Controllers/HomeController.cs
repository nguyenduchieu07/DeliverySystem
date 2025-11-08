using DataAccessLayer.Constants;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Abstractions.IServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

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
        public async Task<IActionResult> Index()
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
            return View(reports);
        }
    }
}
