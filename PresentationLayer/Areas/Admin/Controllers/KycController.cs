using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class KycController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
