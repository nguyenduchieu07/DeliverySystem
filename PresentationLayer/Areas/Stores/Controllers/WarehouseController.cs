using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Areas.Stores.Models.WarehouseModels;

namespace PresentationLayer.Areas.Stores.Controllers
{
    [Area("Stores")]
    public class WarehouseController : Controller
    {
        private readonly DeliverySytemContext _db;
        public WarehouseController(DeliverySytemContext deliverySytemContext)
        {
            _db = deliverySytemContext;
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
                    CreatedAt = w.CreatedAt
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
    }
}
