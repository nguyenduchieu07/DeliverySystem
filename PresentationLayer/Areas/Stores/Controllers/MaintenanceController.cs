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
    public class MaintenanceController : Controller
    {
        private readonly DeliverySytemContext _db;
        public MaintenanceController(DeliverySytemContext db) => _db = db;

        #region Helpers

        private async Task<Guid?> GetCurrentStoreIdAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            if (userId == null) return null;
            if (userRole != null && userRole.Trim().ToUpper().Equals(UserRoles.STORESTAFF.ToUpper()))
            {
                var uid = Guid.Parse(userId);
                return await _db.StoreStaffs
                    .Where(s => s.UserId == uid)
                    .Select(s => s.StoreId)
                    .FirstOrDefaultAsync();
            }
            else
            {
                var uid = Guid.Parse(userId);
                return await _db.Stores
                    .Where(s => s.OwnerUserId == uid)
                    .Select(s => s.Id)
                    .FirstOrDefaultAsync();
            }
        }

        private Task<bool> SlotBelongsToStore(Guid slotId, Guid storeId)
        {
            return _db.WarehouseSlots
                .AnyAsync(s => s.Id == slotId && s.Warehouse.StoreId == storeId);
        }

        private Task<bool> WarehouseBelongsToStore(Guid warehouseId, Guid storeId)
        {
            return _db.Warehouses
                .AnyAsync(w => w.Id == warehouseId && w.StoreId == storeId);
        }

        private Task<bool> MaintenanceBelongsToStore(Guid maintenanceId, Guid storeId)
        {
            return _db.WarehouseSlotMaintenances
                .AnyAsync(m => m.Id == maintenanceId && m.WarehouseSlot.Warehouse.StoreId == storeId);
        }

        #endregion

        // ========== 1) INDEX: Dashboard + Upcoming + History + Warehouses ==========
        public async Task<IActionResult> Index()
        {
            var storeId = await GetCurrentStoreIdAsync();
            if (storeId is null) return Unauthorized();

            // Widgets
            var baseQuery = _db.WarehouseSlotMaintenances
                .Where(x => x.WarehouseSlot.Warehouse.StoreId == storeId);

            ViewBag.Upcoming = await baseQuery.CountAsync(x => x.Status == MaintenanceStatus.Scheduled);
            ViewBag.InProgress = await baseQuery.CountAsync(x => x.Status == MaintenanceStatus.InProgress);
            ViewBag.Overdue = await baseQuery.CountAsync(x => x.Status == MaintenanceStatus.Overdue);
            ViewBag.Completed = await baseQuery.CountAsync(x => x.Status == MaintenanceStatus.Completed);

            // Upcoming (top 50 sắp tới)
            var upcoming = await baseQuery
                .Include(x => x.WarehouseSlot).ThenInclude(s => s.Warehouse)
                .Include(x => x.MaintenanceItem)
                .Where(x => x.Status == MaintenanceStatus.Scheduled)
                .OrderBy(x => x.ScheduledStart)
                .Take(50)
                .ToListAsync();

            // History (gần nhất 50)
            var history = await baseQuery
                .Include(x => x.WarehouseSlot).ThenInclude(s => s.Warehouse)
                .Include(x => x.MaintenanceItem)
                .Where(x => x.Status == MaintenanceStatus.Completed)
                .OrderByDescending(x => x.ActualEnd)
                .Take(50)
                .ToListAsync();

            // Warehouses (toàn bộ kho của store)
            var warehouses = await _db.Warehouses
                .Where(w => w.StoreId == storeId)
                .OrderBy(w => w.Name)
                .ToListAsync();

            var vm = new MaintenanceIndexVM
            {
                Upcoming = upcoming,
                History = history,
                Warehouses = warehouses
            };

            return View(vm);
        }

        // ========== 2) WAREHOUSE: danh sách slot ==========
        public async Task<IActionResult> Warehouse(Guid id)
        {
            var storeId = await GetCurrentStoreIdAsync();
            if (storeId is null) return Unauthorized();

            if (!await WarehouseBelongsToStore(id, storeId.Value)) return NotFound();

            var warehouse = await _db.Warehouses.FindAsync(id);
            if (warehouse == null) return NotFound();

            var slots = await _db.WarehouseSlots
                .Where(s => s.WarehouseId == id)
                .OrderBy(s => s.Row).ThenBy(s => s.Col).ThenBy(s => s.Code)
                .ToListAsync();

            var vm = new MaintenanceWarehouseVM
            {
                Warehouse = warehouse,
                Slots = slots
            };
            return View(vm);
        }

        // ========== 3) SLOT: danh sách bảo trì theo slot + nút CRUD ==========
        public async Task<IActionResult> Slot(Guid id)
        {
            var storeId = await GetCurrentStoreIdAsync();
            if (storeId is null) return Unauthorized();

            if (!await SlotBelongsToStore(id, storeId.Value)) return NotFound();

            var slot = await _db.WarehouseSlots
                .Include(s => s.Warehouse)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (slot == null) return NotFound();

            var maints = await _db.WarehouseSlotMaintenances
                .Include(m => m.MaintenanceItem)
                .Where(m => m.WarehouseSlotId == id)
                .OrderByDescending(m => m.ScheduledStart)
                .ToListAsync();

            var vm = new MaintenanceSlotVM
            {
                Slot = slot,
                Maintenances = maints
            };
            return View(vm);
        }

        // ========== 4) CREATE SCHEDULE ==========
        [HttpGet]
        public async Task<IActionResult> Create(Guid slotId)
        {
            var storeId = await GetCurrentStoreIdAsync();
            if (storeId is null) return Unauthorized();
            if (!await SlotBelongsToStore(slotId, storeId.Value)) return NotFound();

            var items = await _db.MaintenanceItems
                .Where(x => x.Status == StatusValue.Active &&
                            (x.StoreId == null || x.StoreId == storeId))
                .OrderBy(x => x.Name)
                .Select(x => new MaintenanceItemOption { Id = x.Id, Name = x.Name })
                .ToListAsync();

            var vm = new MaintenanceCreateVM
            {
                SlotId = slotId,
                Items = items,
                ScheduledStart = DateTime.Now.AddHours(1),
                ScheduledEnd = DateTime.Now.AddHours(2)
            };
            return View("Form", vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaintenanceCreateVM vm)
        {
            var storeId = await GetCurrentStoreIdAsync();
            if (storeId is null) return Unauthorized();
            if (!await SlotBelongsToStore(vm.SlotId, storeId.Value)) return NotFound();

            // Validate cơ bản
            if (vm.MaintenanceItemId == Guid.Empty)
                ModelState.AddModelError(nameof(vm.MaintenanceItemId), "Vui lòng chọn loại bảo trì.");
            if (vm.ScheduledEnd <= vm.ScheduledStart)
                ModelState.AddModelError(nameof(vm.ScheduledEnd), "Thời gian kết thúc phải sau thời gian bắt đầu.");

            // Chống overlap
            var overlap = await _db.WarehouseSlotMaintenances.AnyAsync(x =>
                x.WarehouseSlotId == vm.SlotId &&
                x.Status != MaintenanceStatus.Cancelled &&
                ((vm.ScheduledStart >= x.ScheduledStart && vm.ScheduledStart < x.ScheduledEnd) ||
                 (vm.ScheduledEnd > x.ScheduledStart && vm.ScheduledEnd <= x.ScheduledEnd) ||
                 (vm.ScheduledStart <= x.ScheduledStart && vm.ScheduledEnd >= x.ScheduledEnd)));

            if (overlap)
                ModelState.AddModelError("", "Khoảng thời gian bảo trì bị chồng lấp với lịch khác.");

            if (!ModelState.IsValid)
            {
                // load lại dropdown
                vm.Items = await _db.MaintenanceItems
                    .Where(x => x.Status == StatusValue.Active && (x.StoreId == null || x.StoreId == storeId))
                    .OrderBy(x => x.Name)
                    .Select(x => new MaintenanceItemOption { Id = x.Id, Name = x.Name })
                    .ToListAsync();
                return View("Form", vm);
            }

            var entity = new WarehouseSlotMaintenance
            {
                Id = Guid.NewGuid(),
                WarehouseSlotId = vm.SlotId,
                MaintenanceItemId = vm.MaintenanceItemId,
                ScheduledStart = vm.ScheduledStart,
                ScheduledEnd = vm.ScheduledEnd,
                Status = MaintenanceStatus.Scheduled,
                Note = vm.Note,
                UpdatedAt = DateTime.UtcNow,
            };

            _db.WarehouseSlotMaintenances.Add(entity);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Đã tạo lịch bảo trì.";
            return RedirectToAction(nameof(Slot), new { id = vm.SlotId });
        }

        // ========== 5) EDIT SCHEDULE ==========
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var storeId = await GetCurrentStoreIdAsync();
            if (storeId is null) return Unauthorized();
            if (!await MaintenanceBelongsToStore(id, storeId.Value)) return NotFound();

            var m = await _db.WarehouseSlotMaintenances
                .Include(x => x.WarehouseSlot)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (m == null || m.Status == MaintenanceStatus.Completed || m.Status == MaintenanceStatus.InProgress)
                return BadRequest("Không thể sửa lịch đã bắt đầu hoặc hoàn thành.");

            var items = await _db.MaintenanceItems
                .Where(x => x.Status == StatusValue.Active && (x.StoreId == null || x.StoreId == storeId))
                .OrderBy(x => x.Name)
                .Select(x => new MaintenanceItemOption { Id = x.Id, Name = x.Name })
                .ToListAsync();

            var vm = new MaintenanceEditVM
            {
                Id = m.Id,
                SlotId = m.WarehouseSlotId,
                MaintenanceItemId = m.MaintenanceItemId,
                ScheduledStart = m.ScheduledStart,
                ScheduledEnd = m.ScheduledEnd,
                Note = m.Note,
                Items = items
            };
            return View("Form", vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MaintenanceEditVM vm)
        {
            var storeId = await GetCurrentStoreIdAsync();
            if (storeId is null) return Unauthorized();

            if (vm.Id is null) return NotFound();

            if (!await MaintenanceBelongsToStore(vm.Id.Value, storeId.Value)) return NotFound();

            var m = await _db.WarehouseSlotMaintenances
                .AsTracking()
                .FirstOrDefaultAsync(x => x.Id == vm.Id);
            if (m == null) return NotFound();
            if (m.Status == MaintenanceStatus.Completed || m.Status == MaintenanceStatus.InProgress)
                return BadRequest("Không thể sửa lịch đã bắt đầu hoặc hoàn thành.");

            if (vm.ScheduledEnd <= vm.ScheduledStart)
                ModelState.AddModelError(nameof(vm.ScheduledEnd), "Thời gian kết thúc phải sau thời gian bắt đầu.");

            var overlap = await _db.WarehouseSlotMaintenances.AnyAsync(x =>
                x.WarehouseSlotId == m.WarehouseSlotId &&
                x.Id != vm.Id &&
                x.Status != MaintenanceStatus.Cancelled &&
                ((vm.ScheduledStart >= x.ScheduledStart && vm.ScheduledStart < x.ScheduledEnd) ||
                 (vm.ScheduledEnd > x.ScheduledStart && vm.ScheduledEnd <= x.ScheduledEnd) ||
                 (vm.ScheduledStart <= x.ScheduledStart && vm.ScheduledEnd >= x.ScheduledEnd)));

            if (overlap)
                ModelState.AddModelError("", "Khoảng thời gian bảo trì bị chồng lấp với lịch khác.");

            if (!ModelState.IsValid)
            {
                vm.Items = await _db.MaintenanceItems
                    .Where(x => x.Status == StatusValue.Active && (x.StoreId == null || x.StoreId == storeId))
                    .OrderBy(x => x.Name)
                    .Select(x => new MaintenanceItemOption { Id = x.Id, Name = x.Name })
                    .ToListAsync();
                return View("Form", vm);
            }

            m.MaintenanceItemId = vm.MaintenanceItemId;
            m.ScheduledStart = vm.ScheduledStart;
            m.ScheduledEnd = vm.ScheduledEnd;
            m.Note = vm.Note;
            m.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            TempData["Success"] = "Đã cập nhật lịch bảo trì.";
            return RedirectToAction(nameof(Slot), new { id = m.WarehouseSlotId });
        }

        // ========== 6) DELETE SCHEDULE ==========
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var storeId = await GetCurrentStoreIdAsync();
            if (storeId is null) return Unauthorized();
            if (!await MaintenanceBelongsToStore(id, storeId.Value)) return NotFound();

            var m = await _db.WarehouseSlotMaintenances
                .AsTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (m == null) return NotFound();
            if (m.Status == MaintenanceStatus.InProgress)
                return BadRequest("Không thể xóa khi đang thực hiện.");
            if (m.Status == MaintenanceStatus.Completed)
                return BadRequest("Không thể xóa lịch đã hoàn thành.");

            var slotId = m.WarehouseSlotId;

            _db.WarehouseSlotMaintenances.Remove(m); // xóa cứng theo yêu cầu
            await _db.SaveChangesAsync();

            TempData["Success"] = "Đã xóa lịch bảo trì.";
            return RedirectToAction(nameof(Slot), new { id = slotId });
        }

        // ========== 7) START ==========
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Start(Guid id)
        {
            var storeId = await GetCurrentStoreIdAsync();
            if (storeId is null) return Unauthorized();
            if (!await MaintenanceBelongsToStore(id, storeId.Value)) return NotFound();

            var m = await _db.WarehouseSlotMaintenances
                .Include(x => x.WarehouseSlot)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (m == null) return NotFound();
            if (m.Status != MaintenanceStatus.Scheduled && m.Status != MaintenanceStatus.Overdue)
                return BadRequest("Chỉ có thể bắt đầu lịch ở trạng thái Scheduled/Overdue.");

            // Không cho start nếu Slot đang InUse
            if (m.WarehouseSlot.Status == StatusValue.InUse)
                return BadRequest("Slot đang InUse. Không thể bắt đầu bảo trì.");

            m.Status = MaintenanceStatus.InProgress;
            m.ActualStart = DateTime.UtcNow;

            // Lock slot: chuyển slot về Maintenance (tùy enum của bạn, ví dụ dùng StatusValue.Maintenance)
            m.WarehouseSlot.Status = StatusValue.Maintenance;

            await _db.SaveChangesAsync();
            TempData["Success"] = "Đã bắt đầu bảo trì.";
            return RedirectToAction(nameof(Slot), new { id = m.WarehouseSlotId });
        }

        // ========== 8) COMPLETE (GET form + POST) ==========
        [HttpGet]
        public async Task<IActionResult> Complete(Guid id)
        {
            var storeId = await GetCurrentStoreIdAsync();
            if (storeId is null) return Unauthorized();
            if (!await MaintenanceBelongsToStore(id, storeId.Value)) return NotFound();

            var m = await _db.WarehouseSlotMaintenances.FindAsync(id);
            if (m == null) return NotFound();
            if (m.Status != MaintenanceStatus.InProgress)
                return BadRequest("Chỉ hoàn thành lịch đang InProgress.");

            var vm = new MaintenanceCompleteVM
            {
                Id = m.Id,
                Result = "",
                Note = m.Note
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CompleteAjax(Guid id, string result, string? note)
        {
            var storeId = await GetCurrentStoreIdAsync();
            if (storeId is null) return Json(new { success = false, message = "Unauthorized" });
            if (!await MaintenanceBelongsToStore(id, storeId.Value))
                return Json(new { success = false, message = "Not found" });

            var m = await _db.WarehouseSlotMaintenances
                .Include(x => x.WarehouseSlot)
                .AsTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (m == null) return Json(new { success = false, message = "Not found" });
            if (m.Status != MaintenanceStatus.InProgress)
                return Json(new { success = false, message = "Chỉ hoàn thành lịch đang InProgress." });

            if (string.IsNullOrWhiteSpace(result))
                return Json(new { success = false, message = "Vui lòng nhập kết quả bảo trì." });

            m.Status = MaintenanceStatus.Completed;
            m.Result = result;
            m.Note = note ?? m.Note;
            m.ActualEnd = DateTime.UtcNow;
            m.UpdatedAt = DateTime.UtcNow;

            // Unlock slot
            m.WarehouseSlot.Status = StatusValue.Available;

            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Đã hoàn thành bảo trì!" });
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(MaintenanceCompleteVM vm)
        {
            var storeId = await GetCurrentStoreIdAsync();
            if (storeId is null) return Unauthorized();
            if (!await MaintenanceBelongsToStore(vm.Id, storeId.Value)) return NotFound();

            var m = await _db.WarehouseSlotMaintenances
                .Include(x => x.WarehouseSlot)
                .AsTracking()
                .FirstOrDefaultAsync(x => x.Id == vm.Id);

            if (m == null) return NotFound();
            if (m.Status != MaintenanceStatus.InProgress)
                return BadRequest("Chỉ hoàn thành lịch đang InProgress.");

            m.Status = MaintenanceStatus.Completed;
            m.ActualEnd = DateTime.UtcNow;
            m.Result = vm.Result;
            m.Note = string.IsNullOrWhiteSpace(vm.Note) ? m.Note : vm.Note;
            m.UpdatedAt = DateTime.UtcNow;

            // Trả slot về Available (hoặc status cũ nếu bạn lưu snapshot)
            m.WarehouseSlot.Status = StatusValue.Available;

            await _db.SaveChangesAsync();
            TempData["Success"] = "Đã hoàn thành bảo trì.";
            return RedirectToAction(nameof(Slot), new { id = m.WarehouseSlotId });
        }

        // ========== 9) CANCEL ==========
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var storeId = await GetCurrentStoreIdAsync();
            if (storeId is null) return Unauthorized();
            if (!await MaintenanceBelongsToStore(id, storeId.Value)) return NotFound();

            var m = await _db.WarehouseSlotMaintenances
                .AsTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (m == null) return NotFound();

            if (m.Status == MaintenanceStatus.InProgress || m.Status == MaintenanceStatus.Completed)
                return BadRequest("Không thể hủy lịch đã bắt đầu hoặc hoàn thành.");

            m.Status = MaintenanceStatus.Cancelled;
            m.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            TempData["Success"] = "Đã hủy lịch bảo trì.";
            return RedirectToAction(nameof(Slot), new { id = m.WarehouseSlotId });
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteAjax(Guid id)
        {
            var storeId = await GetCurrentStoreIdAsync();
            if (storeId is null)
                return Json(new { success = false, message = "Không xác thực được cửa hàng." });

            if (!await MaintenanceBelongsToStore(id, storeId.Value))
                return Json(new { success = false, message = "Không tìm thấy lịch bảo trì." });

            var m = await _db.WarehouseSlotMaintenances
                .AsTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (m == null)
                return Json(new { success = false, message = "Không tồn tại lịch bảo trì." });

            if (m.Status == MaintenanceStatus.InProgress)
                return Json(new { success = false, message = "Không thể xóa khi đang thực hiện." });

            if (m.Status == MaintenanceStatus.Completed)
                return Json(new { success = false, message = "Không thể xóa lịch đã hoàn thành." });

            _db.WarehouseSlotMaintenances.Remove(m);
            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "Đã xóa lịch bảo trì." });
        }


        [HttpPost]
        public async Task<IActionResult> StartAjax(Guid id)
        {
            var storeId = await GetCurrentStoreIdAsync();
            if (storeId is null) return Json(new { success = false, message = "Unauthorized" });
            if (!await MaintenanceBelongsToStore(id, storeId.Value))
                return Json(new { success = false, message = "Not found" });

            var m = await _db.WarehouseSlotMaintenances
                .Include(x => x.WarehouseSlot)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (m == null) return Json(new { success = false, message = "Not found" });
            if (m.Status != MaintenanceStatus.Scheduled && m.Status != MaintenanceStatus.Overdue)
                return Json(new { success = false, message = "Không thể bắt đầu lịch này." });

            if (m.WarehouseSlot.Status == StatusValue.InUse)
                return Json(new { success = false, message = "Slot đang sử dụng, không thể bảo trì." });

            m.Status = MaintenanceStatus.InProgress;
            m.ActualStart = DateTime.UtcNow;
            m.WarehouseSlot.Status = StatusValue.Maintenance;

            await _db.SaveChangesAsync();

            return Json(new
            {
                success = true,
                message = "Đã bắt đầu bảo trì.",
                status = "InProgress"
            });
        }
    }
}

public class MaintenanceIndexVM
{
    public List<WarehouseSlotMaintenance> Upcoming { get; set; } = new();
    public List<WarehouseSlotMaintenance> History { get; set; } = new();
    public List<Warehouse> Warehouses { get; set; } = new();
}

// Warehouse → Slots list
public class MaintenanceWarehouseVM
{
    public Warehouse Warehouse { get; set; } = null!;
    public List<WarehouseSlot> Slots { get; set; } = new();
}

// Slot → Maintenances list
public class MaintenanceSlotVM
{
    public WarehouseSlot Slot { get; set; } = null!;
    public List<WarehouseSlotMaintenance> Maintenances { get; set; } = new();
}

// Dropdown item
public class MaintenanceItemOption
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}

// Complete
public class MaintenanceCompleteVM
{
    public Guid Id { get; set; }
    public string Result { get; set; } = null!;
    public string? Note { get; set; }
}

public class MaintenanceFormBaseVM
{
    public Guid? Id { get; set; } // null = Create, not null = Edit

    public Guid SlotId { get; set; }
    public Guid MaintenanceItemId { get; set; }
    public DateTime ScheduledStart { get; set; }
    public DateTime ScheduledEnd { get; set; }
    public string? Note { get; set; }

    public List<MaintenanceItemOption> Items { get; set; } = new();
}

public class MaintenanceCreateVM : MaintenanceFormBaseVM
{
}

public class MaintenanceEditVM : MaintenanceFormBaseVM
{
}