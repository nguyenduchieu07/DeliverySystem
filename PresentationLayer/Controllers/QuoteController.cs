using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class QuoteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
