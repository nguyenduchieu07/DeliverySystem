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
    [Authorize(Roles = UserRoles.STORE)]
    public class WarehouseController : Controller
    {
        private readonly DeliverySytemContext _db;
        private readonly ICloudinaryService _files;
        public WarehouseController(DeliverySytemContext deliverySytemContext, ICloudinaryService cloudinaryService)
        {
            _db = deliverySytemContext;
            _files = cloudinaryService;
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
                    Status = w.Status
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
        public async Task<IActionResult> Create([FromForm]Warehouse warehouse, [FromForm]Address? newAddress, [FromForm] List<WarehouseSlot> slots, [FromForm] IFormFile? CoverImage, [FromForm] IFormFile? MapImage)
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

            TempData["Success"] = "Tạo kho mới thành công!";
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

            // update cover/map image if re-uploaded
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
            TempData["Success"] = "Cập nhật kho thành công!";
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
        public async Task<IActionResult> Details(Guid id)
        {
            var warehouse = await _db.Warehouses
                .Include(w => w.Address)
                .Include(w => w.Slots)
                .Include(w => w.Store)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (warehouse == null)
            {
                TempData["Error"] = "Warehouse not found!";
                return RedirectToAction("Index");
            }

            return View(warehouse);
        }


    }
}
