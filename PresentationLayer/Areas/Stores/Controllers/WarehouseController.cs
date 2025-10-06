using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Areas.Stores.Models.WarehouseModels;
using ServiceLayer.Abstractions.IServices;

namespace PresentationLayer.Areas.Stores.Controllers
{
    [Area("Stores")]
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
            
            var storeId = Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAA3");
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
                    CoverImageUrl = w.CoverImageUrl
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


        // GET: /Warehouses/Create?storeId=...
        public async Task<IActionResult> Create(Guid storeId)
        {
            await LoadDropdowns();
            return View("Upsert", new WarehouseUpsertDto { StoreId = storeId });
        }

        // GET: /Warehouses/Edit/{id}
        public async Task<IActionResult> Edit(Guid id)
        {
            var w = await _db.Warehouses.Include(x => x.Address).FirstOrDefaultAsync(x => x.Id == id);
            if (w == null) return NotFound();

            await LoadDropdowns();
            return View("Upsert", new WarehouseUpsertDto
            {
                Id = w.Id,
                StoreId = w.StoreId,
                Name = w.Name,
                AddressRefId = w.AddressRefId,
                AddressLine = w.Address?.AddressLine,      // nếu Address có FullAddress
                                                           // Latitude/Longitude chỉ fill nếu Address có các field này:
                Latitude = (w.Address as dynamic)?.Latitude,
                Longitude = (w.Address as dynamic)?.Longitude,
                CoverImageUrl = w.CoverImageUrl,
                MapImageUrl = w.MapImageUrl
            });
        }

        // POST: /Warehouses/Upsert
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(WarehouseUpsertDto vm)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View("Upsert", vm);
            }

            // Nếu chưa chọn AddressRefId và có nhập từ map → thử tạo Address mới (tối thiểu FullAddress)
            if (vm.AddressRefId == null && !string.IsNullOrWhiteSpace(vm.AddressLine))
            {
                // NOTE: chỉnh lại cho đúng model Address của bạn nếu khác tên field!
                var addr = new Address
                {
                    Id = Guid.NewGuid(),
                    AddressLine = vm.AddressLine!,
                };
                // Nếu Address của bạn có lat/lng:
                var dyn = addr as dynamic;
                try
                {
                    dyn.Latitude = vm.Latitude ?? 0d;
                    dyn.Longitude = vm.Longitude ?? 0d;
                }
                catch { /* Address không có lat/lng thì bỏ qua */ }

                _db.Addresses.Add(addr);
                await _db.SaveChangesAsync();
                vm.AddressRefId = addr.Id;
            }

            if (vm.AddressRefId == null)
            {
                ModelState.AddModelError(nameof(vm.AddressRefId), "Chọn địa chỉ có sẵn hoặc tạo mới từ bản đồ.");
                await LoadDropdowns();
                return View("Upsert", vm);
            }

            if (vm.Id == null) // CREATE
            {
                var coverImage = await _files.UploadImageFileAsync(vm.CoverImage);
                var mapImage = await _files.UploadImageFileAsync(vm.MapImage);
                var w = new Warehouse
                {
                    Id = Guid.NewGuid(),
                    StoreId = vm.StoreId,
                    Name = vm.Name,
                    AddressRefId = vm.AddressRefId,
                    CoverImageUrl = coverImage,
                    MapImageUrl = mapImage
                };
                _db.Warehouses.Add(w);
            }
            else // UPDATE
            {
                var w = await _db.Warehouses.FindAsync(vm.Id.Value);
                if (w == null) return NotFound();

                w.StoreId = vm.StoreId;
                w.Name = vm.Name;
                w.AddressRefId = vm.AddressRefId;
                w.CoverImageUrl = vm.CoverImageUrl;
                w.MapImageUrl = vm.MapImageUrl;
                w.UpdatedAt = DateTime.Now;
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "StoreWarehouses", new { storeId = vm.StoreId });
        }

        private async Task LoadDropdowns()
        {
            ViewBag.Stores = await _db.Stores.AsNoTracking().ToListAsync();
            ViewBag.Addresses = await _db.Addresses.AsNoTracking().OrderBy(a => a.AddressLine).ToListAsync();
        }

        // =============== SLOTS (trong cùng controller) ===============

        // GET: /Warehouses/Slots?warehouseId=...
        public async Task<IActionResult> Slots(Guid warehouseId)
        {
            var w = await _db.Warehouses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == warehouseId);
            if (w == null) return NotFound();

            var slots = await _db.WarehouseSlots.AsNoTracking()
                .Where(s => s.WarehouseId == warehouseId)
                .OrderBy(s => s.Code)
                .ToListAsync();

            ViewBag.Warehouse = w;
            return View("Slots", slots);
        }

        // GET: /Warehouses/SlotsCreate?warehouseId=...
        public async Task<IActionResult> SlotsCreate(Guid warehouseId)
        {
            var w = await _db.Warehouses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == warehouseId);
            if (w == null) return NotFound();

            ViewBag.Warehouse = w;
            return View("SlotUpsert", new WarehouseSlotUpsertDto { WarehouseId = warehouseId, Status = "Available" });
        }

        // POST: /Warehouses/SlotsCreate
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> SlotsCreate(WarehouseSlotUpsertDto vm, CancellationToken ct)
        {
            var w = await _db.Warehouses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == vm.WarehouseId, ct);
            if (w == null) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Warehouse = w;
                return View("SlotUpsert", vm);
            }

            string? imageUrl = null;
            if (vm.ImageFile is { Length: > 0 })
            {
                imageUrl = await _files.UploadImageFileAsync(vm.ImageFile);
            }

            var slot = new WarehouseSlot
            {
                Id = Guid.NewGuid(),
                WarehouseId = vm.WarehouseId,
                Code = vm.Code,
                Status = vm.Status,
                Size = vm.Size,
                ImageUrl = imageUrl
            };

            _db.WarehouseSlots.Add(slot);
            await _db.SaveChangesAsync(ct);
            return RedirectToAction(nameof(Slots), new { warehouseId = vm.WarehouseId });
        }

        // GET: /Warehouses/SlotsEdit/{id}
        public async Task<IActionResult> SlotsEdit(Guid id)
        {
            var slot = await _db.WarehouseSlots.Include(s => s.Warehouse).FirstOrDefaultAsync(s => s.Id == id);
            if (slot == null) return NotFound();

            var vm = new WarehouseSlotUpsertDto
            {
                Id = slot.Id,
                WarehouseId = slot.WarehouseId,
                Code = slot.Code,
                Status = slot.Status,
                Size = slot.Size,
                ExistingImageUrl = slot.ImageUrl
            };
            ViewBag.Warehouse = slot.Warehouse;
            return View("SlotUpsert", vm);
        }

        // POST: /Warehouses/SlotsEdit/{id}
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> SlotsEdit(Guid id, WarehouseSlotUpsertDto vm, CancellationToken ct)
        {
            if (id != vm.Id) return BadRequest();

            var slot = await _db.WarehouseSlots.FindAsync(new object?[] { id }, ct);
            if (slot == null) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Warehouse = await _db.Warehouses.FindAsync(slot.WarehouseId);
                return View("SlotUpsert", vm);
            }

            if (vm.ImageFile is { Length: > 0 })
            {
                slot.ImageUrl = await _files.UploadImageFileAsync(vm.ImageFile);
            }

            slot.Code = vm.Code;
            slot.Status = vm.Status;
            slot.Size = vm.Size;
            slot.UpdatedAt = DateTime.Now;

            await _db.SaveChangesAsync(ct);
            return RedirectToAction(nameof(Slots), new { warehouseId = slot.WarehouseId });
        }
    }
}
