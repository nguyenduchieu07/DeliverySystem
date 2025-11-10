using System.ComponentModel.DataAnnotations;
using DataAccessLayer.Constants;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace PresentationLayer.Areas.Stores.Controllers
{
    [Area("Stores")]
    [Authorize(Roles = $"{UserRoles.STORE}, {UserRoles.STORESTAFF}")]
    public class MaintenanceItemController : Controller
    {
        private readonly DeliverySytemContext _db;
        public MaintenanceItemController(DeliverySytemContext db) => _db = db;

        private async Task<Guid?> GetStoreIdAsync()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (userId == null) return null;
            if (userRole != null && userRole.Trim().ToUpper().Equals(UserRoles.STORESTAFF.ToUpper()))
            {
                var guid = Guid.Parse(userId);
                return await _db.StoreStaffs
                    .Where(e => e.UserId == guid)
                    .Select(e => e.StoreId)
                    .FirstOrDefaultAsync();
            }
            else
            {

                var guid = Guid.Parse(userId);
                return await _db.Stores.Where(x => x.OwnerUserId == guid).Select(x => x.Id).FirstOrDefaultAsync();
            }


        }

        // GET: /Stores/MaintenanceItem
        public async Task<IActionResult> Index()
        {
            var storeId = await GetStoreIdAsync();
            if (storeId == null) return Unauthorized();

            var items = await _db.MaintenanceItems
                .Where(x => x.Status == StatusValue.Active && (x.StoreId == null || x.StoreId == storeId))
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            ViewBag.StoreId = storeId;
            return View(items);
        }

        // GET: /Stores/MaintenanceItem/Create
        public async Task<IActionResult> Create()
        {
            var storeId = await GetStoreIdAsync();
            if (storeId == null) return Unauthorized();

            return View(new MaintenanceItemEditVM
            {
                EstimatedDurationMinutes = 60,
                RequireShutdown = false
            });
        }

        // POST: /Stores/MaintenanceItem/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaintenanceItemEditVM vm)
        {
            var storeId = await GetStoreIdAsync();
            if (storeId == null) return Unauthorized();

            if (!ModelState.IsValid) return View(vm);

            var entity = new MaintenanceItem
            {
                Id = Guid.NewGuid(),
                Name = vm.Name,
                Description = vm.Description,
                EstimatedDurationMinutes = vm.EstimatedDurationMinutes,
                RequireShutdown = vm.RequireShutdown,
                Status = StatusValue.Active,
                StoreId = storeId, // Local item (not global)
                CreatedAt = DateTime.UtcNow,
            };

            _db.MaintenanceItems.Add(entity);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Tạo loại bảo trì thành công!";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Stores/MaintenanceItem/Edit/{id}
        public async Task<IActionResult> Edit(Guid id)
        {
            var storeId = await GetStoreIdAsync();
            if (storeId == null) return Unauthorized();

            var entity = await _db.MaintenanceItems
                .FirstOrDefaultAsync(x => x.Id == id && x.StoreId == storeId);

            if (entity == null) return NotFound();

            return View(new MaintenanceItemEditVM
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                EstimatedDurationMinutes = entity.EstimatedDurationMinutes,
                RequireShutdown = entity.RequireShutdown
            });
        }

        // POST: /Stores/MaintenanceItem/Edit
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MaintenanceItemEditVM vm)
        {
            var storeId = await GetStoreIdAsync();
            if (storeId == null) return Unauthorized();

            if (!ModelState.IsValid) return View(vm);
            if (vm.Id == null) return BadRequest();

            var entity = await _db.MaintenanceItems
                .FirstOrDefaultAsync(x => x.Id == vm.Id && x.StoreId == storeId);

            if (entity == null) return NotFound();

            entity.Name = vm.Name;
            entity.Description = vm.Description;
            entity.EstimatedDurationMinutes = vm.EstimatedDurationMinutes;
            entity.RequireShutdown = vm.RequireShutdown;
            entity.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            TempData["Success"] = "Cập nhật loại bảo trì thành công!";
            return RedirectToAction(nameof(Index));
        }

        // POST: /Stores/MaintenanceItem/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var storeId = await GetStoreIdAsync();
            if (storeId == null) return Unauthorized();

            var entity = await _db.MaintenanceItems
                .FirstOrDefaultAsync(x => x.Id == id && x.StoreId == storeId);

            if (entity == null) return NotFound();

            _db.MaintenanceItems.Remove(entity);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Đã xóa loại bảo trì!";
            return RedirectToAction(nameof(Index));
        }
    }
}

public class MaintenanceItemEditVM
{
    public Guid? Id { get; set; }

    [Microsoft.Build.Framework.Required, StringLength(200)]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [Range(1, 1440)] public int EstimatedDurationMinutes { get; set; } = 60;

    public bool RequireShutdown { get; set; }
}