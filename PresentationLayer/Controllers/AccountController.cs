using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Helpers;
using PresentationLayer.Models;
using ServiceLayer.Abstractions.IServices;
using ServiceLayer.Dtos.RegisterStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace PresentationLayer.Controllers
{
    public class AccountController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IStoreRegistrationService _storeService;
        private readonly DeliverySytemContext _db;
        private readonly ICloudinaryService _files;
        public AccountController(
            ICustomerService customerService,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailSender emailSender,
            IStoreRegistrationService storeService,
            DeliverySytemContext db,
            ICloudinaryService files)
        {
            _customerService = customerService;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _storeService = storeService;
            _db = db;
            _files = files;
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
            var fullPhoneNumber = model.PhoneNumber;
            if (!model.PhoneNumber.StartsWith("+") && !model.PhoneNumber.StartsWith("0"))
            {
                fullPhoneNumber = model.CountryCode + model.PhoneNumber;
            }
            if (string.IsNullOrEmpty(model.Email))
            {
                ModelState.AddModelError("Email", "Email là bắt buộc.");
                return View(model);
            }
            var (success, message, user) = await _customerService.RegisterCustomerAsync(
                phoneNumber: fullPhoneNumber,
                password: model.Password,
                fullName: model.FullName,
                email: model.Email
            );
            if (success)
            {
                var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = emailConfirmationToken }, protocol: Request.Scheme);
                await _emailSender.SendEmailAsync(
                    model.Email,
                    "Xác nhận email",
                    $"Vui lòng xác nhận email bằng cách <a href='{callbackUrl}'>nhấn vào đây</a>."
                );
                TempData["SuccessMessage"] = message;
                return RedirectToAction(nameof(Login));
            }
            ModelState.AddModelError(string.Empty, message);
            return View(model);
        }
        #endregion

        #region Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");
            ViewData["ReturnUrl"] = returnUrl;
            if (TempData["SuccessMessage"] != null)
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View(new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
                return View(model);
            var phoneOrEmail = model.PhoneOrEmail.Trim();
            var (success, message) = await _customerService.LoginAsync(
                phoneOrEmail: phoneOrEmail,
                password: model.Password,
                rememberMe: model.RememberMe
            );

            if (success)
            {
                var user = await _userManager.FindByEmailAsync(phoneOrEmail);
                if (user == null) 
                {
                    user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneOrEmail);
                }

                if (user != null && await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
             
                else
                {
                    // Nếu là Customer (hoặc vai trò khác), giữ nguyên logic cũ
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    return RedirectToAction("Index", "Delivery");
                }
                // === KẾT THÚC THAY ĐỔI ===
            }
            ModelState.AddModelError(string.Empty, message);
            return View(model);
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

        #region Forgot Password
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var email = model.Email?.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError("Email", "Email không được để trống.");
                return View(model);
            }
            var (success, message, resetToken) = await _customerService.ForgotPasswordAsync(email);
            if (!success)
            {
                ModelState.AddModelError("", message);
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(email);
            var userId = user?.Id.ToString();
            if (user == null)
            {
                ModelState.AddModelError("", "Không tìm thấy tài khoản.");
                return View(model);
            }
            var encodedToken = System.Web.HttpUtility.UrlEncode(resetToken);
            var callbackUrl = Url.Action(
                "ResetPassword",
                "Account",
                new { userId = userId, code = encodedToken },
                protocol: Request.Scheme
            );
            await _emailSender.SendEmailAsync(
                email,
                "Đặt lại mật khẩu",
                $"Vui lòng đặt lại mật khẩu bằng cách <a href='{callbackUrl}'>nhấn vào đây</a>."
            );
            return RedirectToAction("ForgotPasswordConfirmation");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        #endregion

        #region Reset Password
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string userId, string code)
        {
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(userId))
                return BadRequest("Mã token hoặc userId không hợp lệ.");
            var decodedToken = System.Web.HttpUtility.UrlDecode(code);
            var model = new ResetPasswordViewModel
            {
                UserId = userId,
                Code = decodedToken
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var decodedToken = System.Web.HttpUtility.UrlDecode(model.Code);
            var (success, message) = await _customerService.ResetPasswordAsync(
                model.UserId!,
                decodedToken!,
                model.NewPassword!
            );
            if (success)
                return RedirectToAction("ResetPasswordConfirmation");
            ModelState.AddModelError(string.Empty, message);
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        #endregion

        #region Change Password
        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordViewModel());
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            var (success, message) = await _customerService.ChangePasswordAsync(userId, model.CurrentPassword, model.NewPassword);
            if (success)
            {
                ViewBag.Success = message;
                return View();
            }
            ModelState.AddModelError(string.Empty, message);
            return View(model);
        }
        #endregion

        #region Profile
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            var customer = await _customerService.GetProfileAsync(userId);
            var addresses = await _customerService.GetAddressesAsync(userId);
            var orders = await _customerService.GetOrdersAsync(userId);
            var model = new ProfileViewModel
            {
                Customer = customer ?? new Customer(),
                Addresses = addresses?.Select(a => new AddressViewModel
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    StoreId = a.StoreId,
                    Label = a.Label,
                    AddressLine = a.AddressLine,
                    Ward = a.Ward,
                    District = a.District,
                    City = a.City,
                    Latitude = a.Latitude,
                    Longitude = a.Longitude,
                    IsDefault = a.IsDefault,
                    Active = a.Active
                }).ToList() ?? new List<AddressViewModel>(),
                Orders = orders,
                NewAddress = new AddressViewModel()
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
                var customer = await _customerService.GetProfileAsync(userId);
                model.Customer = customer ?? new Customer();
                model.Addresses = (await _customerService.GetAddressesAsync(userId))?.Select(a => new AddressViewModel
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    StoreId = a.StoreId,
                    Label = a.Label,
                    AddressLine = a.AddressLine,
                    Ward = a.Ward,
                    District = a.District,
                    City = a.City,
                    Latitude = a.Latitude,
                    Longitude = a.Longitude,
                    IsDefault = a.IsDefault,
                    Active = a.Active
                }).ToList() ?? new List<AddressViewModel>();
                model.Orders = await _customerService.GetOrdersAsync(userId);
                model.NewAddress = model.NewAddress ?? new AddressViewModel();
                return View("Profile", model);
            }
            var userIdUpdate = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            try
            {
                await _customerService.UpdateProfileAsync(
                    userIdUpdate,
                    model.Customer.FullName,
                    model.Customer.Email,
                    model.Customer.PhoneNumber,
                    model.Customer.PreferredLang,
                    model.Customer.Tier
                );
                TempData["SuccessMessage"] = "Cập nhật hồ sơ thành công.";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var customer = await _customerService.GetProfileAsync(userIdUpdate);
                model.Customer = customer ?? new Customer();
                model.Addresses = (await _customerService.GetAddressesAsync(userIdUpdate))?.Select(a => new AddressViewModel
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    StoreId = a.StoreId,
                    Label = a.Label,
                    AddressLine = a.AddressLine,
                    Ward = a.Ward,
                    District = a.District,
                    City = a.City,
                    Latitude = a.Latitude,
                    Longitude = a.Longitude,
                    IsDefault = a.IsDefault,
                    Active = a.Active
                }).ToList() ?? new List<AddressViewModel>();
                model.Orders = await _customerService.GetOrdersAsync(userIdUpdate);
                model.NewAddress = model.NewAddress ?? new AddressViewModel();
                return View("Profile", model);
            }
            return RedirectToAction("Profile");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAddress(ProfileViewModel model)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            if (string.IsNullOrWhiteSpace(model.NewAddress?.AddressLine))
            {
                ModelState.AddModelError("NewAddress.AddressLine", "Địa chỉ không được để trống.");
                var customer = await _customerService.GetProfileAsync(userId);
                model.Customer = customer ?? new Customer();
                model.Addresses = (await _customerService.GetAddressesAsync(userId))?.Select(a => new AddressViewModel
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    StoreId = a.StoreId,
                    Label = a.Label,
                    AddressLine = a.AddressLine,
                    Ward = a.Ward,
                    District = a.District,
                    City = a.City,
                    Latitude = a.Latitude,
                    Longitude = a.Longitude,
                    IsDefault = a.IsDefault,
                    Active = a.Active
                }).ToList() ?? new List<AddressViewModel>();
                model.Orders = await _customerService.GetOrdersAsync(userId);
                model.NewAddress = model.NewAddress ?? new AddressViewModel();
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
            try
            {
                await _customerService.AddAddressAsync(userId, address);
                TempData["SuccessMessage"] = "Thêm địa chỉ thành công.";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var customer = await _customerService.GetProfileAsync(userId);
                model.Customer = customer ?? new Customer();
                model.Addresses = (await _customerService.GetAddressesAsync(userId))?.Select(a => new AddressViewModel
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    StoreId = a.StoreId,
                    Label = a.Label,
                    AddressLine = a.AddressLine,
                    Ward = a.Ward,
                    District = a.District,
                    City = a.City,
                    Latitude = a.Latitude,
                    Longitude = a.Longitude,
                    IsDefault = a.IsDefault,
                    Active = a.Active
                }).ToList() ?? new List<AddressViewModel>();
                model.Orders = await _customerService.GetOrdersAsync(userId);
                model.NewAddress = model.NewAddress ?? new AddressViewModel();
                return View("Profile", model);
            }
            return RedirectToAction("Profile");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAddress(Guid addressId)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            try
            {
                await _customerService.DeleteAddressAsync(addressId);
                TempData["SuccessMessage"] = "Xóa địa chỉ thành công.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("Profile");
        }
        #endregion

        #region Social Login (Placeholders)
        [HttpGet]
        [AllowAnonymous]
        public IActionResult FacebookLogin()
        {
            TempData["ErrorMessage"] = "Đăng nhập bằng Facebook chưa được hỗ trợ";
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GoogleLogin()
        {
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
        #endregion

        #region Confirm Email
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
            {
                TempData["ErrorMessage"] = "UserId hoặc mã xác nhận không hợp lệ.";
                return RedirectToAction("ConfirmEmailConfirmation");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy tài khoản.";
                return RedirectToAction("ConfirmEmailConfirmation");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Xác nhận email thành công.";
                return RedirectToAction("ConfirmEmailConfirmation");
            }
            TempData["ErrorMessage"] = "Xác nhận email thất bại.";
            return RedirectToAction("ConfirmEmailConfirmation");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ConfirmEmailConfirmation()
        {
            return View();
        }
        #endregion

        #region Role Management
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AssignRole(string userId)
        {
            ViewBag.UserId = userId;
            var roles = new List<string> { "Admin", "Manager", "Staff", "User" }; // Thay bằng truy vấn thực tế nếu cần
            ViewBag.Roles = roles;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, role);
            return RedirectToAction("Profile", new { userId });
        }
        #endregion


        #region store
        [HttpGet]
        [Authorize]
        public IActionResult RegisterStore()
        {
            ViewBag.Addresses = _db.Addresses
                .Where(a => a.Active)
                .OrderByDescending(a => a.CreatedAt)
                .ToList();
            return View(new RegisterStoreRequest());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RegisterStore(RegisterStoreRequest request)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                request.UserId = Guid.Parse(userId!);
                var data = await _storeService.RegisterStoreAsync(request);
                var dto = new KycViewModel
                {
                    Response = data,
                };
                HttpContext.Session.SetObject("kyc", dto);
                return View("RegisterWarehouse");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(RegisterStore), request);
            }

        }
        [HttpGet]
        [Authorize]
        public IActionResult RegisterWarehouse()
        {
            ViewBag.Addresses = _db.Addresses
                .Where(a => a.Active)
                .OrderByDescending(a => a.CreatedAt)
                .ToList();
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RegisterWarehouse([FromForm] Warehouse warehouse, [FromForm] Address? newAddress, [FromForm] List<WarehouseSlot> slots, [FromForm] IFormFile? CoverImage, [FromForm] IFormFile? MapImage)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var id = Guid.Parse(userId!);
            var storeId = await _db.Stores.Where(e => e.OwnerUserId == id).Select(e => e.Id).FirstOrDefaultAsync();
            warehouse.StoreId = storeId;
            var dto = HttpContext.Session.GetObject<KycViewModel>("kyc");
            if (CoverImage != null && CoverImage.Length > 0)
            {
                var coverImage = await _files.UploadImageFileAsync(CoverImage);
                warehouse.CoverImageUrl = coverImage;
            }
            if (MapImage != null && MapImage.Length > 0)
            {
                var mapImage = await _files.UploadImageFileAsync(MapImage);
                warehouse.MapImageUrl = mapImage;
            }
            // Nếu người dùng nhập địa chỉ mới → tạo mới Address
            if (warehouse.AddressRefId == null && newAddress != null && !string.IsNullOrEmpty(newAddress.AddressLine))
            {
                newAddress.Id = Guid.NewGuid();
                newAddress.Active = true;
                _db.Addresses.Add(newAddress);
                await _db.SaveChangesAsync();
                warehouse.AddressRefId = newAddress.Id;
            }
            warehouse.Id = Guid.NewGuid();
            warehouse.Status = DataAccessLayer.Enums.StatusValue.Approved;
            await _db.Warehouses.AddAsync(warehouse);
            //if (slots != null && slots.Count > 0)
            //{
            //    foreach (var s in slots)
            //    {
            //        s.Id = Guid.NewGuid();
            //        s.WarehouseId = warehouse.Id;
            //        _db.WarehouseSlots.Add(s);
            //    }
            //}

            await _db.SaveChangesAsync();
            return View("KycSubmissions",dto);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SubmitKyc(SubmitKycRequest request)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var id = Guid.Parse(userId!);
                var storeId = await _db.Stores.Where(e => e.OwnerUserId == id).Select(e => e.Id).FirstOrDefaultAsync();
                request.StoreId = storeId;
                var data = await _storeService.SubmitKycDocumentsAsync(request);
                return Ok("Gửi thành công.");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View("KycSubmissions", new KycViewModel
                {
                    KycRequest = request,
                });
            }
        }
        
        #endregion
    }
}