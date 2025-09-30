using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using ServiceLayer.Abstractions.IServices;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PresentationLayer.Controllers
{
    public class AccountController : Controller
    {
        private readonly ICustomerService _customerService;

        public AccountController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        #region Register

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            return View(new RegisterViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Chuẩn hóa số điện thoại
            var fullPhoneNumber = model.PhoneNumber;
            if (!model.PhoneNumber.StartsWith("+") && !model.PhoneNumber.StartsWith("0"))
            {
                fullPhoneNumber = model.CountryCode + model.PhoneNumber;
            }

            // Gọi service để đăng ký
            var (success, message, user) = await _customerService.RegisterCustomerAsync(
                phoneNumber: fullPhoneNumber,
                password: model.Password,
                fullName: model.FullName,
                email: model.Email
            );

            if (success)
            {
                TempData["SuccessMessage"] = message;
                return RedirectToAction(nameof(Login));
            }

            ViewBag.ErrorMessage = message;
            return View(model);
        }

        #endregion

        #region Login (placeholder)


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            // Redirect if already logged in
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["ReturnUrl"] = returnUrl;

            // Display success message from registration if available
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Prepare phone/email for login
            var phoneOrEmail = model.PhoneOrEmail.Trim();

            // If it's not email and doesn't start with +, add country code
            if (!phoneOrEmail.Contains("@") && !phoneOrEmail.StartsWith("+"))
            {
                phoneOrEmail = model.CountryCode + phoneOrEmail;
            }

            // Call service to login
            var (success, message) = await _customerService.LoginAsync(
                phoneOrEmail: phoneOrEmail,
                password: model.Password,
                rememberMe: model.RememberMe
            );

            if (success)
            {
                // Redirect to return URL or home
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Delivery");
                }
            }
            else
            {
                ViewBag.ErrorMessage = message;
                return View(model);
            }
        }

        #endregion

        #region Logout

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _customerService.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Forgot Password (Placeholder)

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPassword(string phoneOrEmail)
        {
            // TODO: Implement forgot password logic
            ViewBag.SuccessMessage = "Chức năng đang được phát triển";
            return View();
        }

        #endregion

        #region Social Login Placeholders

        [HttpGet]
        [AllowAnonymous]
        public IActionResult FacebookLogin()
        {
            // TODO: Implement Facebook OAuth
            TempData["ErrorMessage"] = "Đăng nhập bằng Facebook chưa được hỗ trợ";
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GoogleLogin()
        {
            // TODO: Implement Google OAuth
            TempData["ErrorMessage"] = "Đăng nhập bằng Google chưa được hỗ trợ";
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult FacebookRegister()
        {
            TempData["ErrorMessage"] = "Đăng ký bằng Facebook chưa được hỗ trợ";
            return RedirectToAction(nameof(Register));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GoogleRegister()
        {
            TempData["ErrorMessage"] = "Đăng ký bằng Google chưa được hỗ trợ";
            return RedirectToAction(nameof(Register));
        }
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            var customer = await _customerService.GetProfileAsync(userId);
            var addresses = await _customerService.GetAddressesAsync(userId);
            var orders = await _customerService.GetOrdersAsync(userId);

            var model = new ProfileViewModel
            {
                Customer = customer,
                Addresses = addresses,
                Orders = orders,
                NewAddress = new Address() // Thêm để hỗ trợ form thêm địa chỉ
            };
            return View(model);
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            // Lấy email và phoneNumber từ model nếu có, nếu không giữ nguyên giá trị hiện tại
            var customer = await _customerService.GetProfileAsync(userId) ?? new Customer();
            await _customerService.UpdateProfileAsync(userId, model.Customer.FullName, customer.Email, customer.PhoneNumber, model.Customer.PreferredLang, model.Customer.Tier);
            return RedirectToAction("Profile");
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Profile", model); // Trả về view với model để hiển thị lỗi
            }

            var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            if (string.IsNullOrWhiteSpace(model.NewAddress.AddressLine))
            {
                ModelState.AddModelError("NewAddress.AddressLine", "Địa chỉ không được để trống.");
                return View("Profile", model);
            }

            var address = new Address
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                AddressLine = model.NewAddress.AddressLine,
                City = model.NewAddress.City,
                District = model.NewAddress.District,
                Ward = model.NewAddress.Ward,
                Label = model.NewAddress.Label
            };
            await _customerService.AddAddressAsync(userId, address);
            return RedirectToAction("Profile");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAddress(Guid addressId)
        {
            var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            await _customerService.DeleteAddressAsync(addressId);
            return RedirectToAction("Profile");
        }
    }

    #endregion
}
