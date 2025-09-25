using DataAccessLayer.Abstractions.IRepositories;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Areas.Stores.Models;

namespace PresentationLayer.Areas.Stores.Controllers
{
    [Area("Stores")]
    public class ServiceController : Controller
    {
        private readonly IBaseRepository<Service,Guid> _serviceRepository;
        private readonly Guid _storeId = Guid.Parse("22222222-2222-2222-2222-222222222222"); // demo
        public ServiceController(IBaseRepository<Service,Guid> baseRepository)
        {
            _serviceRepository = baseRepository;
        }
        public async Task<IActionResult> Index()
        {
            var query = await _serviceRepository.FindAll(e => e.StoreId == _storeId,
                e => e.Category
                ).Select(s => new ServiceListItemVM
                {
                    Id = s.Id,
                    Name = s.Name,
                    CategoryName = s.Category != null ? s.Category.Name : null,
                    Unit = s.Unit,
                    BasePrice = s.BasePrice,
                    IsActive = s.IsActive,
                    IsPublished = EF.Property<bool>(s, "IsActive") == true,
                    CreatedAt = s.CreatedAt
                }).ToListAsync();

            return View(query);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var vm = new ServiceEditVM { StoreId = _storeId, IsActive = true, IsPublished = false };
            return View(vm);
        }
    }
}
