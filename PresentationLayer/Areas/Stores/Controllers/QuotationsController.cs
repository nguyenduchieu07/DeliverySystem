using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.Stores.Controllers
{
    public class QuotationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
