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
        private readonly IBaseRepository<Service, Guid> _serviceRepository;
        private readonly IBaseRepository<Category, Guid> _categoryRepository;
        private readonly Guid _storeId = Guid.Parse("22222222-2222-2222-2222-222222222222"); // demo
        public ServiceController(IBaseRepository<Service, Guid> baseRepository, IBaseRepository<Category, Guid> baseRepository1)
        {
            _serviceRepository = baseRepository;
            _categoryRepository = baseRepository1;
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

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var s = await _serviceRepository
                .FindSingleAsync((e => e.Id == id && e.StoreId == _storeId),
                    CancellationToken.None,
                    e => e.ServicePrices);
            if (s == null)
            {
                return NotFound();
            }
            Guid? parentId = null;
            if (s.CategoryId.HasValue)
            {
                parentId = await _categoryRepository
                    .FindAll(e => e.Id == s.CategoryId.Value)
                    .Select(c => c.ParentId)
                    .FirstOrDefaultAsync();

            }
            var vm = new ServiceEditVM
            {
                Id = s.Id,
                StoreId = s.StoreId,
                ParentCategoryId = parentId,
                CategoryId = s.CategoryId,
                Name = s.Name,
                Description = s.Description,
                Unit = s.Unit,
                BasePrice = s.BasePrice,
                IsActive = s.IsActive,
                IsPublished = EF.Property<bool>(s, "IsActive")
            };
            vm.Tiers = s.ServicePrices.OrderByDescending(p => p.ValidFrom).Select(p => new PriceTierVM
            {
                Id = p.Id,
                ServiceId = p.ServiceId,
                ValidFrom = p.ValidFrom,
                ValidTo = p.ValidTo,
                Price = p.Price,
                MinQty = p.MinQty,
                MaxQty = p.MaxQty
            }).ToList();
            return View(vm);
        }
    }
}
