using System.ComponentModel.DataAnnotations;
using DataAccessLayer.Abstractions.IRepositories;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;

namespace PresentationLayer.Areas.Admin.Controllers;


[Area("Admin")]
[Authorize(Roles = "Admin")]
public class StoreController : Controller
{
    private readonly IBaseRepository<Store, Guid> _storeRepository;

    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IKycRepository _kycRepository;

    public StoreController(IBaseRepository<Store, Guid> storeRepository, IFeedbackRepository feedbackRepository, IKycRepository kycRepository)
    {
        _storeRepository = storeRepository;
        _feedbackRepository = feedbackRepository;
        _kycRepository = kycRepository;
    }

    public async Task<IActionResult> Index()
    {
        var stores = await _storeRepository.GetAllAsync();
        return View(stores);
    }
    
    public async Task<IActionResult> Detail(Guid storeId)
    {
        var vm = new AdminStoreDetailViewModel();
        if (storeId == Guid.Empty)
            return NotFound();

        var feedbacks = await _feedbackRepository.GetAllFeedbackByStoreIdAsync(storeId);
        vm.ReviewCount = feedbacks.Count();
        
        var store = await _storeRepository.GetByIdAsync(storeId);
        vm.Store = store;
        
        var kycSubmission = await _kycRepository.GetKycSubmissionByStoreId(storeId);
        vm.KycSubmission = kycSubmission;
        
        return View(vm);
    }
    
    public async Task<IActionResult> GetFeedbacksByStoreAsync(Guid storeId)
    {
        var feedbacks = await _feedbackRepository.GetAllFeedbackByStoreIdAsync(storeId);

        return View(feedbacks);
    }

    public class UpdateStoreDto
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Tên cửa hàng là bắt buộc.")]
        public string StoreName { get; set; }
        [Required(ErrorMessage = "Tên cửa hàng là bắt buộc.")]
        public string LegalName { get; set; }
        [Required(ErrorMessage = "LicenseNumber là bắt buộc.")]
        public string LicenseNumber { get; set; }
        [Required(ErrorMessage = "Mã số thuế là bắt buộc.")]
        public string? TaxNumber { get; set; }
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [StringLength(255)]
        public string? ContactEmail { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string? ContactPhone { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string? Email { get; set; }
        public DateTime? LicenseExpiryDate { get; set; }
        public int? MaxOrdersPerDay { get; set; }
        public string? ActiveRegions { get; set; }
        public string? ServiceTypes { get; set; }
        
        public string? BankAccountNumber { get; set; }
        public string? BankName { get; set; }
        // public double? Latitude { get; set; }
        // public double? Longitude { get; set; }
        
        public StatusValue Status { get; set; }
    }
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var store = await _storeRepository.GetByIdAsync(id);
        if (store == null)
            return NotFound();

        var dto = new UpdateStoreDto
        {
            StoreName = store.StoreName,
            LegalName = store.LegalName,
            LicenseNumber = store.LicenseNumber,
            TaxNumber = store.TaxNumber,
            ContactPhone = store.ContactPhone,
            ContactEmail = store.ContactEmail,
            LicenseExpiryDate = store.LicenseExpiryDate,
            MaxOrdersPerDay = store.MaxOrdersPerDay,
            ActiveRegions = store.ActiveRegions,
            ServiceTypes = store.ServiceTypes,
            BankAccountNumber = store.BankAccountNumber,
            BankName = store.BankName,
            Status = store.Status,
        };

        return PartialView("_EditModal", dto);
    }

    // POST: Store/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateStoreDto model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(ms => ms.Value.Errors.Any())
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            return Json(new { success = false, errors });
        }

        var store = await _storeRepository.GetByIdAsync(id);
        if (store == null)
            return Json(new { success = false, message = "Store not found" });

        // Cập nhật các trường cơ bản
        store.StoreName = model.StoreName;
        store.LegalName = model.LegalName;
        store.LicenseNumber = model.LicenseNumber;
        store.TaxNumber = model.TaxNumber;
        store.ContactPhone = model.ContactPhone;
        store.ContactEmail = model.ContactEmail;
        store.LicenseExpiryDate = model.LicenseExpiryDate;
        store.MaxOrdersPerDay = model.MaxOrdersPerDay;
        store.ActiveRegions = model.ActiveRegions;
        store.ServiceTypes = model.ServiceTypes;
        store.BankAccountNumber = model.BankAccountNumber;
        store.BankName = model.BankName;
        store.Status = model.Status;
        // store.Latitude = model.Latitude;
        // store.Longitude = model.Longitude;

        _storeRepository.Update(store);

        return Json(new { success = true });
    }
}