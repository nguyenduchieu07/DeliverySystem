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
            var id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3");
            var reports =await _dashboardService.GetDashboard(id);
            if(reports == null)
            {
                reports = new Models.DashboardDto();
            }
            return View(reports);
        }
    }
}
