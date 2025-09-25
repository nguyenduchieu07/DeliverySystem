using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.Stores.Controllers
{
    [Area("Stores")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
