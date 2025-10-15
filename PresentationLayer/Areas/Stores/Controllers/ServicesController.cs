using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Areas.Stores.Models.Services;
using System;
using System.Security.Claims;

namespace PresentationLayer.Areas.Stores.Controllers
{
    [Area("Stores")]
    [Authorize]
    public class ServicesController : Controller
    {
        private readonly DeliverySytemContext _db;
        public ServicesController(DeliverySytemContext db) => _db = db;

        // GET: /Stores/Services
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var id = Guid.Parse(userId!);
            var storeId = await _db.Stores.Where(e => e.OwnerUserId == id).Select(e => e.Id).FirstOrDefaultAsync();
            var services = await _db.Services
                .Include(s => s.Category)
                .Where(s => s.StoreId == storeId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            ViewBag.StoreId = id;
            return View(services);
        }

        // GET: /Stores/Services/Create
        public IActionResult Create(Guid storeId)
        {
            var vm = new ServiceEditViewModel
            {
                StoreId = storeId,
                IsActive = true,
                PricingModel = PricingModel.TimeBased,
                Unit = "slot"
            };
            return View(vm);
        }

        // POST: /Stores/Services/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceEditViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var entity = new Service
            {
                Id = Guid.NewGuid(),
                StoreId = vm.StoreId,
                CategoryId = vm.CategoryId,
                Name = vm.Name,
                Description = vm.Description,
                Unit = vm.Unit,
                BasePrice = vm.BasePrice,
                PricingModel = vm.PricingModel,
                IsActive = vm.IsActive,
                Status = StatusValue.Active
            };

            // Map SizeOptions
            foreach (var size in vm.SizeOptions)
            {
                entity.SizeOptions.Add(new ServiceSizeOption
                {
                    Id = Guid.NewGuid(),
                    Code = size.Code,
                    DisplayName = size.DisplayName,
                    VolumeM3 = size.VolumeM3,
                    AreaM2 = size.AreaM2,
                    MaxWeightKg = size.MaxWeightKg,
                    PriceOverride = size.PriceOverride
                });
            }

            // Map PriceRules
            foreach (var rule in vm.PriceRules)
            {
                entity.PriceRules.Add(new ServicePriceRule
                {
                    Id = Guid.NewGuid(),
                    ValidFrom = rule.ValidFrom,
                    ValidTo = rule.ValidTo,
                    MinVolumeM3 = rule.MinVolumeM3,
                    MaxVolumeM3 = rule.MaxVolumeM3,
                    MinAreaM2 = rule.MinAreaM2,
                    MaxAreaM2 = rule.MaxAreaM2,
                    Price = rule.Price,
                    ApplyModel = rule.ApplyModel,
                    TimeUnit = rule.TimeUnit
                    
                });
            }

            // map add on

            foreach(var add in vm.Addons)
            {
                entity.Addons.Add(new ServiceAddon
                {
                    Id = Guid.NewGuid(),
                    IsPercentage = add.IsPercentage,
                    Name = add.Name,
                    Value = add.Value,
                    Description = add.Description,                    
                });
            }
            _db.Services.Add(entity);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Tạo dịch vụ thành công!";
            return RedirectToAction(nameof(Index), new { storeId = vm.StoreId });
        }

        // GET: /Stores/Services/Edit/{id}
        public async Task<IActionResult> Edit(Guid storeId, Guid id)
        {
            var s = await _db.Services
                .Include(x => x.SizeOptions)
                .Include(x => x.PriceRules)
                .Include(x => x.Addons)
                .FirstOrDefaultAsync(x => x.Id == id && x.StoreId == storeId);

            if (s == null) return NotFound();

            var vm = new ServiceEditViewModel
            {
                Id = s.Id,
                StoreId = s.StoreId,
                CategoryId = s.CategoryId,
                Name = s.Name,
                Description = s.Description,
                Unit = s.Unit,
                BasePrice = s.BasePrice,
                PricingModel = s.PricingModel,
                IsActive = s.IsActive,
                SizeOptions = s.SizeOptions.Select(o => new ServiceSizeOptionVM(o)).ToList(),
                PriceRules = s.PriceRules.Select(r => new ServicePriceRuleVM(r)).ToList(),
                Addons = s.Addons.Select(a => new ServiceAddonVM(a)).ToList()
            };

            return View(vm);
        }

        // POST: /Stores/Services/Edit
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ServiceEditViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            if (vm.Id == null) return BadRequest();

            var s = await _db.Services
                .AsTracking()
                .FirstOrDefaultAsync(x => x.Id == vm.Id && x.StoreId == vm.StoreId);

            if (s == null) return NotFound();

            // 3) Map scalar
            s.Name = vm.Name?.Trim();
            s.Description = vm.Description?.Trim();
            s.Unit = vm.Unit?.Trim();
            s.BasePrice = vm.BasePrice;
            s.PricingModel = vm.PricingModel;
            s.IsActive = vm.IsActive;
            s.CategoryId = vm.CategoryId;

            await _db.ServiceSizeOptions.Where(x => x.ServiceId == s.Id).ExecuteDeleteAsync();
            await _db.PriceRules.Where(x => x.ServiceId == s.Id).ExecuteDeleteAsync();
            await _db.ServiceAddons.Where(x => x.ServiceId == s.Id).ExecuteDeleteAsync();
            foreach (var o in (vm.SizeOptions ?? Enumerable.Empty<ServiceSizeOptionVM>()))
            {
                _db.ServiceSizeOptions.Add(new ServiceSizeOption
                {
                    Id = o.Id ?? Guid.NewGuid(),
                    ServiceId = s.Id, 
                    Code = o.Code?.Trim() ?? string.Empty,
                    DisplayName = o.DisplayName?.Trim() ?? string.Empty,
                    VolumeM3 = o.VolumeM3,
                    AreaM2 = o.AreaM2,
                    MaxWeightKg = o.MaxWeightKg,
                    PriceOverride = o.PriceOverride
                });
            }

            // 6) CHÈN LẠI PriceRules
            foreach (var r in (vm.PriceRules ?? Enumerable.Empty<ServicePriceRuleVM>()))
            {
                _db.PriceRules.Add(new ServicePriceRule
                {
                    Id = r.Id ?? Guid.NewGuid(),
                    ServiceId = s.Id,                           
                    ValidFrom = r.ValidFrom,
                    ValidTo = r.ValidTo,
                    MinVolumeM3 = r.MinVolumeM3,
                    MaxVolumeM3 = r.MaxVolumeM3,
                    MinAreaM2 = r.MinAreaM2,
                    MaxAreaM2 = r.MaxAreaM2,
                    MinQty = r.MinQty,
                    MaxQty = r.MaxQty,
                    MinDays = r.MinDays,
                    MaxDays = r.MaxDays,
                    TimeUnit = r.TimeUnit,
                    ApplyModel = r.ApplyModel,
                    Price = r.Price
                });
            }
            // 7) CHÈN LẠI Addons
            foreach (var a in (vm.Addons ?? Enumerable.Empty<ServiceAddonVM>()))
            {
                _db.ServiceAddons.Add(new ServiceAddon
                {
                    Id = a.Id ?? Guid.NewGuid(),
                    ServiceId = s.Id,                           // <-- GÁN RÕ
                    Name = a.Name?.Trim() ?? string.Empty,
                    Description = a.Description?.Trim(),
                    IsPercentage = a.IsPercentage,
                    Value = a.Value
                });
            }
            try
            {
                await _db.SaveChangesAsync();
                TempData["Success"] = "Cập nhật thành công!";
                return RedirectToAction(nameof(Index), new { storeId = vm.StoreId });
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError("", "Bản ghi đã bị thay đổi hoặc xóa bởi người khác. Vui lòng tải lại trang và thử lại.");
                return View(vm);
            }
        }
        // GET: /Stores/Services/Delete/{id}
        public async Task<IActionResult> Delete(Guid storeId, Guid id)
        {
            var s = await _db.Services.FirstOrDefaultAsync(x => x.Id == id && x.StoreId == storeId);
            if (s == null) return NotFound();

            _db.Services.Remove(s);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Đã xóa dịch vụ.";
            return RedirectToAction(nameof(Index), new { storeId });
        }
    }
}
