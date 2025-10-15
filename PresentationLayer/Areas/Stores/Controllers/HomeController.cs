using DataAccessLayer.Constants;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Abstractions.IServices;
using System.Security.Claims;

namespace PresentationLayer.Areas.Stores.Controllers
{
    [Area("Stores")]
    [Authorize(Roles = UserRoles.STORE)]
    public class HomeController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly DeliverySytemContext _db;
        public HomeController(IDashboardService dashboardService, DeliverySytemContext db)
        {
            _dashboardService = dashboardService;
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var id = Guid.Parse(userId!);
            var store = await _db.Stores.Where(e => e.OwnerUserId == id).FirstOrDefaultAsync();
            if(store.Status == DataAccessLayer.Enums.StatusValue.Pending)
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
