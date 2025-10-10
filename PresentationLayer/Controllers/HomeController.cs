using DataAccessLayer.Entities; // <-- THÊM DÒNG NÀY
using Microsoft.AspNetCore.Identity; // <-- THÊM DÒNG NÀY
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using System.Diagnostics;

namespace PresentationLayer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager; // <-- THÊM DÒNG NÀY

        // SỬA LẠI HÀM CONSTRUCTOR NÀY
        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager)
        {
            _logger = logger;
            _userManager = userManager; // <-- THÊM DÒNG NÀY
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // ================================================================
        // === DÁN TOÀN BỘ PHƯƠNG THỨC NÀY VÀO ===
        public IActionResult GetCorrectHash()
        {
            var password = "Admin@123";
            var dummyUser = new User { UserName = "temp" };
            var correctHash = _userManager.PasswordHasher.HashPassword(dummyUser, password);

            // In ra console để dễ debug
            System.Diagnostics.Debug.WriteLine($"Correct Hash for '{password}' is: {correctHash}");

            // Trả về chuỗi hash trên màn hình trình duyệt
            return Content(correctHash);
        }
        // ================================================================
    }
}