using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Abstractions.IServices;

namespace PresentationLayer.Areas.Stores.Controllers
{
    [Area("Stores")]
    public class HomeController : Controller
    {
        private readonly IDashboardService _dashboardService;
        public HomeController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        public async Task<IActionResult> Index()
        {
            var id = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var reports =await _dashboardService.GetDashboard(id);
            if(reports == null)
            {
                reports = new Models.DashboardDto();
            }
            return View(reports);
        }
    }
}
