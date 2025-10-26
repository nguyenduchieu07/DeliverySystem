using ClosedXML.Excel;
using DataAccessLayer.Constants;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Areas.Stores.Models.WarehouseModels;
using ServiceLayer.Abstractions.IServices;
using System.Security.Claims;

namespace PresentationLayer.Areas.Stores.Controllers
{
    [Area("Stores")]
    //[Authorize(Roles = UserRoles.STORE)]
    public class WarehouseController : Controller
    {
        private readonly DeliverySytemContext _db;
        private readonly ICloudinaryService _files;
        private readonly IWarehouseSlotImportService _import;
        private readonly IWarehouseSlotExportService _export;
        public WarehouseController(DeliverySytemContext deliverySytemContext,
            ICloudinaryService cloudinaryService,
            IWarehouseSlotImportService import,
            IWarehouseSlotExportService export)
        {
            _db = deliverySytemContext;
            _files = cloudinaryService;
            _import = import;
            _export = export;
        }
        public async Task<IActionResult> Index([FromQuery] string? q, [FromQuery] Guid? addressId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var id = Guid.Parse(userId!);
            var storeId = await _db.Stores.Where(e => e.OwnerUserId == id).Select(e => e.Id).FirstOrDefaultAsync();
            // Base query: chỉ lấy kho thuộc store
            var query = _db.Warehouses
                .AsNoTracking()
                .Where(w => w.StoreId == storeId)
                .Include(w => w.Address)
                .Include(w => w.Slots)
                .AsQueryable();

            // Search theo tên kho hoặc địa chỉ
            if (!string.IsNullOrWhiteSpace(q))
            {
                var qNorm = q.Trim().ToLower();
                query = query.Where(w =>
                    EF.Functions.Like(w.Name.ToLower(), $"%{qNorm}%") ||
                    (w.Address != null && EF.Functions.Like(w.Address.AddressLine.ToLower(), $"%{qNorm}%")));
            }

            // Filter theo AddressId (optional)
            if (addressId.HasValue && addressId.Value != Guid.Empty)
            {
                query = query.Where(w => w.AddressRefId == addressId.Value);
            }

            // Tổng bản ghi
            var total = await query.CountAsync();

            // Paging (an toàn)
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 5, 50);

            var items = await query
                .OrderByDescending(w => w.CreatedAt)
                .ThenBy(w => w.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(w => new WarehouseListVm.Row
                {
                    Id = w.Id,
                    Name = w.Name,
                    AddressText = w.Address != null ? w.Address.AddressLine : "(No address)",
                    SlotCount = w.Slots.Count,
                    CreatedAt = w.CreatedAt,
                    CoverImageUrl = w.CoverImageUrl,
                    Status = w.Status,
                    HeightM = w.HeightM,
                    LengthM = w.LengthM,
                    WidthM = w.WidthM
                })
                .ToListAsync();

            // Dữ liệu dropdown địa chỉ để lọc (chỉ trong store)
            var addrOptions = await _db.Warehouses
                .AsNoTracking()
                .Where(w => w.StoreId == storeId && w.Address != null)
                .Select(w => new { w.AddressRefId, Name = w.Address!.AddressLine })
                .Distinct()
                .OrderBy(x => x.Name)
                .ToListAsync();

            var vm = new WarehouseListVm
            {
                StoreId = storeId,
                Q = q,
                AddressId = addressId,
                Page = page,
                PageSize = pageSize,
                Total = total,
                Items = items,
                AddressOptions = addrOptions.Select(x => (x.AddressRefId!.Value, x.Name)).ToList()
            };

            ViewBag.Title = "Warehouses";
            return View(vm);
        }



        // GET: /Warehouse/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Addresses = _db.Addresses
                .Where(a => a.Active)
                .OrderByDescending(a => a.CreatedAt)
                .ToList();

            return View();
        }

        // POST: /Warehouse/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Warehouse warehouse, [FromForm] Address? newAddress, [FromForm] List<WarehouseSlot> slots, [FromForm] IFormFile? CoverImage, [FromForm] IFormFile? MapImage)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var id = Guid.Parse(userId!);
            var storeId = await _db.Stores.Where(e => e.OwnerUserId == id).Select(e => e.Id).FirstOrDefaultAsync();
            warehouse.StoreId = storeId;
            //if (!ModelState.IsValid)
            //{
            //    ViewBag.Addresses = _db.Addresses.ToList();
            //    return View(warehouse);
            //}
            try
            {
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
                warehouse.Status = DataAccessLayer.Enums.StatusValue.Pending;
                _db.Warehouses.Add(warehouse);

                // Lưu tạm danh sách slot (nếu có)
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
                await ReindexWarehouseSlotsAsync(warehouse.Id);
                TempData["Success"] = "Tạo kho mới thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Tạo mới kho thất bại {ex.Message}";
            }
            return RedirectToAction("Index");

        }


        // GET: /Warehouse/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var warehouse = await _db.Warehouses
                .Include(w => w.Address)
                .Include(w => w.Slots)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (warehouse == null)
                return NotFound();

            ViewBag.Addresses = await _db.Addresses
                .Where(a => a.Active)
                .ToListAsync();

            return View(warehouse);
        }

        // POST: /Warehouse/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(Warehouse warehouse, Address? newAddress, List<WarehouseSlot> slots, IFormFile? CoverImage, IFormFile? MapImage)
        {
            var existing = await _db.Warehouses
                .Include(w => w.Slots)
                .FirstOrDefaultAsync(w => w.Id == warehouse.Id);

            if (existing == null)
                return NotFound();

            existing.Name = warehouse.Name;

            try
            {
                // update cover/map image if re-uploaded
                if (CoverImage != null && CoverImage.Length > 0)
                {
                    var coverImage = await _files.UploadImageFileAsync(CoverImage);
                    existing.CoverImageUrl = coverImage;
                }
                if (MapImage != null && MapImage.Length > 0)
                {
                    var mapImage = await _files.UploadImageFileAsync(MapImage);
                    existing.MapImageUrl = mapImage;
                }

                // Update Address (tự chọn hoặc tạo mới)
                if (warehouse.AddressRefId == null && newAddress != null && !string.IsNullOrEmpty(newAddress.AddressLine))
                {
                    newAddress.Id = Guid.NewGuid();
                    newAddress.Active = true;
                    _db.Addresses.Add(newAddress);
                    await _db.SaveChangesAsync();
                    existing.AddressRefId = newAddress.Id;
                }
                else
                {
                    existing.AddressRefId = warehouse.AddressRefId;
                }

                // Cập nhật Slot (xoá hết và thêm lại)
                _db.WarehouseSlots.RemoveRange(existing.Slots);
                if (slots != null && slots.Count > 0)
                {
                    foreach (var s in slots)
                    {
                        s.Id = Guid.NewGuid();
                        s.WarehouseId = existing.Id;
                        _db.WarehouseSlots.Add(s);
                    }
                }

                await _db.SaveChangesAsync();
                await ReindexWarehouseSlotsAsync(warehouse.Id);
                TempData["Success"] = "Cập nhật kho thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Cập nhật kho thất bại! {ex.Message}";
            }
            return RedirectToAction("Index");

        }

        // GET: /Warehouse/Delete/{id}
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var warehouse = await _db.Warehouses.FindAsync(id);
            if (warehouse == null) return NotFound();

            _db.Warehouses.Remove(warehouse);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Đã xoá kho thành công!";
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> GetSlotsPaged(Guid warehouseId, string? q, int page = 1, int pageSize = 10)
        {
            var query = _db.WarehouseSlots
                .AsNoTracking()
                .Where(s => s.WarehouseId == warehouseId);

            // Tìm kiếm theo code
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(s => EF.Functions.Like(s.Code.ToLower(), $"%{q.ToLower()}%"));
            }

            var total = await query.CountAsync();

            var slots = await query
                .OrderBy(s => s.Row)
                .ThenBy(s => s.Col)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Json(new
            {
                slots,
                total,
                page,
                pageSize,
                totalPages = (int)Math.Ceiling(total / (double)pageSize)
            });
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id, string? slotSearch, int slotPage = 1, int slotPageSize = 10)
        {
            var warehouse = await _db.Warehouses
                .Include(w => w.Address)
                .Include(w => w.Store)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (warehouse == null)
            {
                TempData["Error"] = "Warehouse not found!";
                return RedirectToAction("Index");
            }

            // Query slots với phân trang
            var slotsQuery = _db.WarehouseSlots
                .Where(s => s.WarehouseId == id);

            if (!string.IsNullOrWhiteSpace(slotSearch))
            {
                slotsQuery = slotsQuery.Where(s =>
                    EF.Functions.Like(s.Code.ToLower(), $"%{slotSearch.ToLower()}%"));
            }

            var totalSlots = await slotsQuery.CountAsync();

            warehouse.Slots = await slotsQuery
                .OrderBy(s => s.Row)
                .ThenBy(s => s.Col)
                .Skip((slotPage - 1) * slotPageSize)
                .Take(slotPageSize)
                .ToListAsync();

            ViewBag.SlotSearch = slotSearch;
            ViewBag.SlotPage = slotPage;
            ViewBag.SlotPageSize = slotPageSize;
            ViewBag.TotalSlots = totalSlots;
            ViewBag.TotalPages = (int)Math.Ceiling(totalSlots / (double)slotPageSize);

            return View(warehouse);
        }

        [HttpGet]
        public async Task<IActionResult> Grid(Guid warehouseId)
        {
            var slots = await _db.WarehouseSlots.Where(x => x.WarehouseId == warehouseId).ToListAsync();
            if (!slots.Any()) return Json(new { rows = 0, cols = 0, cells = Array.Empty<object>() });

            int rows = slots.Max(x => x.Row);
            int cols = slots.Max(x => x.Col);
            var today = DateTime.Today;

            int WarningOf(WarehouseSlot s)
            {
                if (s.IsBlocked) return 3; // blocked
                if (s.LeaseEnd is DateTime e && e < today) return 2; // expired
                if (s.LeaseEnd is DateTime e2 && (e2 - today).TotalDays <= 7) return 1; // expiring
                if (s.LeaseStart == null && s.LeaseEnd == null) return 4; // empty
                return 0; // none
            }

            var cells = slots.Select(s => new {
                s.Code,
                s.Row,
                s.Col,
                Warning = WarningOf(s),
                PricePreview = s.IsBlocked ? (decimal?)null : s.BasePricePerHour,
                s.IsBlocked
            });

            return Json(new { rows, cols, cells });
        }

        // Import Excel theo template
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportSlots(Guid warehouseId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "File rỗng.";
                return RedirectToAction("Details", new { id = warehouseId });
            }

            using var stream = file.OpenReadStream();
            var result = await _import.ImportAsync(stream);

            if (result.Errors.Any())
            {
                TempData["Error"] = $"Import lỗi {result.Errors.Count} dòng / {result.TotalRows}.";
            }
            else
            {
                TempData["Success"] = $"Import thành công {result.Success} dòng.";
            }

            return RedirectToAction("Details", new { id = warehouseId });
        }
        public static class WarehouseLayoutOptions
        {
            public const int MaxColsPerRow = 30;
        }

        public async Task ReindexWarehouseSlotsAsync(Guid warehouseId)
        {
            var slots = await _db.Set<WarehouseSlot>()
                .Where(s => s.WarehouseId == warehouseId)
                .OrderBy(s => s.CreatedAt)
                .ToListAsync();

            int maxCols = WarehouseLayoutOptions.MaxColsPerRow;
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].Row = (i / maxCols) + 1;
                slots[i].Col = (i % maxCols) + 1;
            }
            await _db.SaveChangesAsync();
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSlotTemplate(CancellationToken ct)
        {
            var bytes = await _export.ExportTemplateAsync(ct);
            var fileName = $"SlotTemplate_{DateTime.UtcNow:yyyyMMdd}.xlsx";
            return File(bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }

        [HttpGet]
        public async Task<IActionResult> ExportSlots(Guid id, CancellationToken ct) // id = warehouseId
        {
            var bytes = await _export.ExportWarehouseSlotsAsync(id, ct);
            var fileName = $"WarehouseSlots_{id}_{DateTime.UtcNow:yyyyMMdd}.xlsx";
            return File(bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }

        // (tuỳ chọn) xuất nhiều kho
        [HttpPost]
        public async Task<IActionResult> ExportMultiple([FromForm] Guid[] warehouseIds, CancellationToken ct)
        {
            if (warehouseIds == null || warehouseIds.Length == 0)
                return BadRequest("Chọn ít nhất 1 kho.");

            var bytes = await _export.ExportMultiWarehousesAsync(warehouseIds, ct);
            var fileName = $"WarehouseSlots_{DateTime.UtcNow:yyyyMMdd}.xlsx";
            return File(bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }
    }
}
