using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Abstractions.IServices;
using ServiceLayer.Dtos.RegisterStore;

namespace PresentationLayer.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IStoreRegistrationService _storeService;
        public AccountsController(IStoreRegistrationService storeRegistrationService)
        {
            _storeService = storeRegistrationService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RegisterStore()
        {
            return View(new RegisterStoreRequest());
        }

        [HttpPost]
        public async Task<IActionResult> RegisterStore(RegisterStoreRequest request)
        {
            try
            {
                var data = await _storeService.RegisterStoreAsync(request);
                var dto = new KycViewModel
                {
                    Response = data,
                };
                return View("KycSubmissions", dto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(RegisterStore),request);
            }

        }


        [HttpPost]
        public async Task<IActionResult> SubmitKyc(SubmitKycRequest request)
        {
            try
            {
                var data = await _storeService.SubmitKycDocumentsAsync(request);
                return Ok("Gửi thành công."); 
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View("KycSubmissions", request);
            }
        }
        
        
    }
}
