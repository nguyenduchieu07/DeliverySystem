using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace PresentationLayer.Controllers { [Authorize] // Yêu cầu đăng nhập
 public class DeliveryController : Controller 
    { 
        public IActionResult Index() { 
            return View(); 
        } 
        public IActionResult Orders() {
            return View();
        } // Thêm các action khác nếu cần
         public IActionResult BookDelivery() { 
            return View(); 
        } 
    }
}