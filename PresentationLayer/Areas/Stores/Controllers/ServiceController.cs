using DataAccessLayer.Abstractions.IRepositories;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Areas.Stores.Helpers;
using PresentationLayer.Areas.Stores.Models;

namespace PresentationLayer.Areas.Stores.Controllers
{
    [Area("Stores")]
    public class ServiceController : Controller
    {
        private readonly IBaseRepository<Service, Guid> _serviceRepository;
        private readonly IBaseRepository<Category, Guid> _categoryRepository;
        private readonly DeliverySytemContext _context;
        private readonly Guid _storeId = Guid.Parse("22222222-2222-2222-2222-222222222222"); // demo
        public ServiceController(IBaseRepository<Service, Guid> baseRepository, IBaseRepository<Category, Guid> baseRepository1, DeliverySytemContext context)
        {
            _serviceRepository = baseRepository;
            _categoryRepository = baseRepository1;
            _context = context;

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
                IsPublished = true
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceEditVM vm)
        {
            if (!ModelState.IsValid) return View(vm);
            if (vm.CategoryId == null) { ModelState.AddModelError("CategoryId", "Hãy chọn danh mục cuối trong nhánh"); return View(vm); }

            var entity = new Service
            {
                Id = Guid.NewGuid(),
                StoreId = _storeId,
                CategoryId = vm.CategoryId,
                Name = vm.Name,
                Description = vm.Description,
                Unit = vm.Unit,
                BasePrice = vm.BasePrice,
                IsActive = vm.IsActive,
                CreatedAt = DateTime.UtcNow
            };
            _serviceRepository.Add(entity);
            _context.SaveChanges();
            return RedirectToAction(nameof(Edit), new { id = entity.Id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ServiceEditVM vm)
        {
            if (!ModelState.IsValid) return View(vm);
            if (vm.CategoryId == null) { ModelState.AddModelError("CategoryId", "Hãy chọn danh mục cuối trong nhánh"); return View(vm); }

            var s = await _serviceRepository.FindAll(x => x.Id == vm.Id && x.StoreId == _storeId).FirstOrDefaultAsync();
            if (s == null) return NotFound();

            s.CategoryId = vm.CategoryId; s.Name = vm.Name; s.Description = vm.Description; s.Unit = vm.Unit; s.BasePrice = vm.BasePrice; s.IsActive = vm.IsActive;
            _context.Attach(s).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Edit), new { vm.Id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTier(PriceTierVM vm)
        {

            var service = await _serviceRepository
                .FindAll(s => s.Id == vm.ServiceId && s.StoreId == _storeId,
                e => e.ServicePrices)
                .FirstOrDefaultAsync();
            if (service == null) return NotFound();


            var newTier = new ServicePrice
            {
                Id = Guid.NewGuid(),
                ServiceId = service.Id,
                ValidFrom = vm.ValidFrom.Date,
                ValidTo = vm.ValidTo.Value,
                Price = vm.Price,
                MinQty = vm.MinQty,
                MaxQty = vm.MaxQty,
                CreatedAt = DateTime.UtcNow
            };

            // Validate chồng chéo
            var errors = PriceTierValidator.ValidateNewTier(newTier, service.ServicePrices).ToList();
            if (errors.Any())
            {
                TempData["TierErrors"] = string.Join("\n", errors);
                return RedirectToAction(nameof(Edit), new { id = vm.ServiceId });
            }

            _context.ServicePrices.Add(newTier);
            await _context.SaveChangesAsync();
            TempData["TierSuccess"] = "Đã thêm tier.";
            return RedirectToAction(nameof(Edit), new { id = vm.ServiceId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTier(Guid serviceId, Guid tierId)
        {
            
            TempData["TierSuccess"] = "Đã xoá tier.";
            return RedirectToAction(nameof(Edit), new { id = serviceId });
        }
    }
}
