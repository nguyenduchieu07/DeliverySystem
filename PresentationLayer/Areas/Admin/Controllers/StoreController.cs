using DataAccessLayer.Abstractions.IRepositories;
using DataAccessLayer.Entities;
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
    public StoreController(IBaseRepository<Store, Guid> storeRepository, IFeedbackRepository feedbackRepository)
    {
        _storeRepository = storeRepository;
        _feedbackRepository = feedbackRepository;
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
        
        return View(vm);
    }
    
    public async Task<IActionResult> GetFeedbacksByStoreAsync(Guid storeId)
    {
        var feedbacks = await _feedbackRepository.GetAllFeedbackByStoreIdAsync(storeId);

        return View(feedbacks);
    }
}