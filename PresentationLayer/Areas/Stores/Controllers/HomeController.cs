using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.Stores.Controllers
{
    [Area("Stores")]
    public class HomeController : Controller
    {
        
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
